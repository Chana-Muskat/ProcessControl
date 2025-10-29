using Microsoft.AspNetCore.Mvc;
using ProcessControl.Server.Models.Ramdor;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessControl.Server.Data;
using Newtonsoft.Json;
using Azure.Core;
using System.Xml.Serialization;

namespace ProcessControl.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamIvController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public RamIvController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {

                using (var connection = _context.Database.GetDbConnection())
                {
                    connection.Open();
                    return Ok("✅ חיבור לדטה-בייס עובד תקין!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ שגיאה בחיבור: {ex.Message}");
            }
        }

        // GET: api/<RamIvController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "חשבונית 1", "חשבונית 2" };

        }
        // GET: api/invoices
        [HttpGet("getInvoices")]
        public async Task<IActionResult> GetInvoices(
            [FromQuery] string? searchTerm,
            [FromQuery] string? selectedDate,
            [FromQuery] string? selectedStatus,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 30)
        {
            try
            {
                Console.WriteLine($"Received Request: skip={skip}, take={take}");
                var query = _context.RamInvoices.Include(i => i.StatusIvId).AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(i => i.WorkNum.ToString().Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(selectedDate))
                {
                    DateTime dateFilter = DateTime.Parse(selectedDate);
                    query = query.Where(i => i.StartDate.HasValue && i.StartDate.Value.Date == dateFilter.Date);
                }

                if (!string.IsNullOrEmpty(selectedStatus))
                {
                    query = query.Where(i => i.StatusIvId.SDes == selectedStatus);
                }

                var invoices = await query
                    .Where(i => i.Type == "I")
                    .OrderBy(i =>
            i.StatusIv == 3 ? 0 :
            i.StatusIv == 1 ? 1 :
            2)
        .ThenByDescending(i => i.StartDate)
                    .Skip(skip)
                    .Take(take)
                    .Select(i => new
                    {
                        InvoiceId = i.InvoiceId,
                        WorkNum = i.WorkNum,
                        StartDate = i.StartDate,
                        Version = i.Version,
                        StatusIv = i.StatusIv,
                        StatusDescription = i.StatusIvId.SDes
                    })
                    .ToListAsync();

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getIvStages/{invoiceId}/stages")]
        public async Task<ActionResult> GetInvoiceStages(int invoiceId)
        {
            var ivStages = await _context.RamStagesPerIv
                .Include(s => s.InvoiceId)
                .Include(s => s.StageId)
                .Where(s => s.InvoiceIdf == invoiceId)
                .Select(s => new
                {
                    s.StageIv,
                    s.InvoiceIdf,
                    s.StageIdf,
                    s.ResStatus,
                    s.UDate,
                    ErrDes = s.ErrDes != null ? s.ErrDes : "no error",
                    IvRequest = s.IvRequest != null ? s.IvRequest : "no request",
                    StageDes = s.StageId.StageDes
                })
                .OrderBy(s => s.StageIdf)
                .ToListAsync();
            return Ok(ivStages);
        }

        [HttpPost("newInvoice")]
        public async Task<IActionResult> newInvoice([FromBody] dynamic data)
        {
            var invoiceJson = JsonConvert.SerializeObject(data);
            var invoice = JsonConvert.DeserializeObject<RamInvoices>(invoiceJson);

            if (invoice == null)
                return BadRequest("החשבונית ריקה!");
            Console.WriteLine("Received Invoice:", JsonConvert.SerializeObject(invoice));

            invoice.StatusIv = 1;
            invoice.Type = "I";
            _context.RamInvoices.Add(invoice);
            await _context.SaveChangesAsync();

            string invoiceJsons = JsonConvert.SerializeObject(invoice);

            var newStages = new List<RamStagesPerIv> {
                new RamStagesPerIv
            {
                InvoiceIdf = invoice.InvoiceId,  // עכשיו ה-ID זמין
                StageIdf = 1,                    // שלב ראשוני קבוע
                UDate = DateTime.UtcNow,         // תאריך כניסה
                ResStatus = "SUCCESS",
               // ErrDes = "חשבונית התקבלה למערכת",
              // IvRequest = invoiceJsons
            },
                new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 2,                    // שלב שני
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        },
        new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 3,                    // שלב שלישי
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        },
        new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 4,                    // שלב רביעי
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        }
    };

            _context.RamStagesPerIv.AddRange(newStages);
            await _context.SaveChangesAsync();

            return Ok(new { message = "חשבונית נוספה בהצלחה!", newInvoice = invoiceJson/*invoiceId = invoice.InvoiceId*/ });
        }

        [HttpPost("updateInvoiceStages")]
        public async Task<IActionResult> updateInvoiceStages([FromBody] RamUpdateStage data)
        {
            var stageJson = JsonConvert.SerializeObject(data);
         
            if (stageJson == null || string.IsNullOrEmpty(data.WorkNum) || data.StageNum == 0)
                return BadRequest("missing data");


            var invoice = await _context.RamInvoices
       .Include(i => i.RamStagePerIv)
       .FirstOrDefaultAsync(i => i.WorkNum == data.WorkNum && i.Version == data.Version && i.Type == data.Type);

            if (invoice == null)
                return NotFound("Not found");

            // שליפת שלב מתוך טבלת השלבים לפי StageNum ו-Type
            var stage = await _context.RamIvStages
                .FirstOrDefaultAsync(s => s.StageNum == data.StageNum && s.Type == data.Type);

            if (stage == null)
                return NotFound("Stage definition not found");


            // חיפוש השלב הרלוונטי במסד הנתונים
            var existingStage = await _context.RamStagesPerIv
       .FirstOrDefaultAsync(s => s.InvoiceIdf == invoice.InvoiceId && s.StageIdf == stage.StageId);

            if (existingStage == null)
                return NotFound("Stage not found for the requested invoice");
            

            // עדכון הנתונים הקיימים
            existingStage.UDate = DateTime.UtcNow;  // עדכון תאריך
            existingStage.ResStatus = "SUCCESS";  // עדכון סטטוס

            // אם השלב הוא 2, עדכן את IvRequest, אחרת השאר אותו ללא שינוי
            if (data.StageNum == 2)
            {
                existingStage.IvRequest = data.IvRequest;
            }
            /* Change iv Status to final after the last stage */
            if (stage.IsFinal == "Y")
            {
                invoice.StatusIv = 2;
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Stage updated successfully", updatedStage = existingStage });

        }
        [HttpPost("markAsDone")]
        public async Task<IActionResult> MarkAsDone([FromBody] dynamic data)
        {
            int invoiceId = data.invoiceId;
            var invoice = await _context.RamInvoices.FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
            if (invoice == null)
                return NotFound("חשבונית לא נמצאה");

            invoice.StatusIv = 2;
            await _context.SaveChangesAsync();

            return Ok(new { message = "החשבונית עודכנה ל'הסתיים'" });
        }
        // GET: api/orders
        [HttpGet("getOrders")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? searchTerm,
            [FromQuery] string? selectedDate,
            [FromQuery] string? selectedStatus,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 30)
        {
            try
            {
                Console.WriteLine($"Received Request: skip={skip}, take={take}");
                var query = _context.RamInvoices.Include(i => i.StatusIvId).AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(i => i.WorkNum.ToString().Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(selectedDate))
                {
                    DateTime dateFilter = DateTime.Parse(selectedDate);
                    query = query.Where(i => i.StartDate.HasValue && i.StartDate.Value.Date == dateFilter.Date);
                }

                if (!string.IsNullOrEmpty(selectedStatus))
                {
                    query = query.Where(i => i.StatusIvId.SDes == selectedStatus);
                }

                var invoices = await query
                    .Where(i => i.Type == "O") // Filter for orders
                    .OrderBy(i =>
            i.StatusIv == 3 ? 0 :
            i.StatusIv == 1 ? 1 :
            2)
        .ThenByDescending(i => i.StartDate)
                    .Skip(skip)
                    .Take(take)
                    .Select(i => new
                    {
                        InvoiceId = i.InvoiceId,
                        WorkNum = i.WorkNum,
                        StartDate = i.StartDate,
                        Version = i.Version,
                        StatusIv = i.StatusIv,
                        Type = i.Type,
                        StatusDescription = i.StatusIvId.SDes
                    })
                    .ToListAsync();

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("newOrder")]
        public async Task<IActionResult> newOrder([FromBody] dynamic data)
        {
            var invoiceJson = JsonConvert.SerializeObject(data);
            var invoice = JsonConvert.DeserializeObject<RamInvoices>(invoiceJson);

            if (invoice == null)
                return BadRequest("חוזה ריק!");
            Console.WriteLine("Received Invoice:", JsonConvert.SerializeObject(invoice));

            invoice.StatusIv = 1;
            invoice.Type = "O";
            _context.RamInvoices.Add(invoice);
            await _context.SaveChangesAsync();

            string invoiceJsons = JsonConvert.SerializeObject(invoice);

            var newStages = new List<RamStagesPerIv> {
                new RamStagesPerIv
            {
                InvoiceIdf = invoice.InvoiceId,  // עכשיו ה-ID זמין
                StageIdf = 5,                    // שלב ראשוני קבוע
                UDate = DateTime.UtcNow,         // תאריך כניסה
                ResStatus = "SUCCESS",
               // ErrDes = "חשבונית התקבלה למערכת",
              // IvRequest = invoiceJsons
            },
                new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 6,                    // שלב שני
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        },
        new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 7,                    // שלב שלישי
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        },
        new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 8,                    // שלב רביעי
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        },
          new RamStagesPerIv
        {
            InvoiceIdf = invoice.InvoiceId,
            StageIdf = 9,                    // שלב חמישי
            UDate = null,                     // עדיין אין תאריך
            ResStatus = null,                 // מחכה לעדכון
            IvRequest = null
        }
    };

            _context.RamStagesPerIv.AddRange(newStages);
            await _context.SaveChangesAsync();

            return Ok(new { message = "חוזה נוסף בהצלחה!", newInvoice = invoiceJson/*invoiceId = invoice.InvoiceId*/ });
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProcessControl.Server.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProcessControl.Server.Models.Ramdor;

namespace ProcessControl.Server.Services
{
    public class RamIvStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RamIvStatusBackgroundService> _logger;

        private readonly TimeSpan _delay = TimeSpan.FromHours(1); // כל שעה

        public RamIvStatusBackgroundService(IServiceProvider serviceProvider, ILogger<RamIvStatusBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateStatusesAsync();
                //await Task.Delay(_delay, stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // 1 דקה במקום שעה
            }
        }

        private async Task UpdateStatusesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var emailService  = scope.ServiceProvider.GetRequiredService<EmailService>();

                // var weekAgo = DateTime.Now.AddDays(-30);
                var now = DateTime.Now;
                var stuckInStage1Limit = now.AddDays(-20);
                var stuckOtherStagesLimit = now.AddDays(-2);

                // Stage 1 stuck over 20 days
                var stuckStage1 = await db.RamStagesPerIv
                    .Include(s => s.InvoiceId)
                    .Include(s => s.StageId)
                    .Where(s => s.StageIdf == 1 &&
                                s.UDate < stuckInStage1Limit &&
                                s.InvoiceId.StatusIv == 1)
                    .ToListAsync();

                // Stage 2 stuck over 2 days (stage 3 doesn't exist yet)
                var stuckStage2 = await db.RamStagesPerIv
                    .Include(s => s.InvoiceId)
                    .Include(s => s.StageId)
                    .Where(s => s.StageIdf == 2 &&
                                s.UDate < stuckOtherStagesLimit &&
                                s.InvoiceId.StatusIv == 1 &&
                                !db.RamStagesPerIv.Any(next => next.InvoiceIdf == s.InvoiceIdf && next.StageIdf == 3))
                    .ToListAsync();

                // Stage 3 stuck over 2 days (stage 4 doesn't exist yet)
                var stuckStage3 = await db.RamStagesPerIv
                    .Include(s => s.InvoiceId)
                    .Include(s => s.StageId)
                    .Where(s => s.StageIdf == 3 &&
                                s.UDate < stuckOtherStagesLimit &&
                                s.InvoiceId.StatusIv == 1 &&
                                !db.RamStagesPerIv.Any(next => next.InvoiceIdf == s.InvoiceIdf && next.StageIdf == 4))
                    .ToListAsync();

                // Combine all stuck
                var allStuckStages = stuckStage1.Concat(stuckStage2).Concat(stuckStage3).ToList();

                if (!allStuckStages.Any())
                {
                    _logger.LogInformation("No stuck invoices found.");
                    return;
                }

                // Update invoice status to error
                foreach (var stage in allStuckStages)
                {
                    stage.InvoiceId.StatusIv = 3;// סטטוס שגיאה
                    _logger.LogInformation($"עודכנה חשבונית לסטטוס שגיאה (ID: {stage.InvoiceId.WorkNum})");
                }

                string emailBody = BuildEmailBody(allStuckStages.Select(s=>(s.InvoiceId, s)).ToList());

                try
                {
                    await emailService.SendEmailAsync(
                        "chanamu@npa.org.il", // כתובת אליה נשלח
                        "דוח יומי: חשבוניות שעברו לשגיאה",
                        emailBody
                    );

                    _logger.LogInformation($"Email sent with {allStuckStages.Count} stuck invoices.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending stuck invoice report.");
                }
                await db.SaveChangesAsync();
            }
        }
        private string BuildEmailBody(List<(RamInvoices Invoice, RamStagesPerIv StuckStage)> stuckInvoices)
        {
            var html = @"
        <div style='direction: rtl; font-family: Arial, sans-serif;'>
            <h2 style='color: #f44336;'>דוח יומי: חשבוניות תקועות בשלבים</h2>
            <p>התאריך: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>
            <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%; text-align: right;'>
                <thead style='background-color: #f44336; color: white;'>
                    <tr>
                        <th>חשבונית</th>
                        <th>תאריך התחלה</th>
                        <th>שלב תקוע</th>
                        <th>תיאור שלב</th>
                    </tr>
                </thead>
                <tbody>";

            foreach (var (invoice, stage) in stuckInvoices)
            {
                html += $@"
                    <tr>
                        <td>{invoice.Version + '_' + invoice.WorkNum}</td>
                        <td>{invoice.StartDate?.ToString("dd/MM/yyyy")}</td>
                        <td>{stage.StageId.StageNum}</td>
                        <td>{stage.StageId.StageDes}</td>
                    </tr>";
            }

            html += @"
                </tbody>
            </table>
            <p>אנא טפלו בחשבוניות אלו בהקדם.</p>
        </div>";

            return html;
        }


    }
}


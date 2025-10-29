using Microsoft.AspNetCore.Mvc;
using ProcessControl.Server.Data;

namespace ProcessControl.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamOrdController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RamOrdController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}

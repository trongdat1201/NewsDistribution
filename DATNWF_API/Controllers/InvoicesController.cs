using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;

namespace DATNWF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public InvoicesController(ThanhnienContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabHoadon>>> GetInvoices()
        {
            if (_context.TabHoadons == null)
            {
                return NotFound();
            }
            return await _context.TabHoadons.ToListAsync();
        }
    }
}

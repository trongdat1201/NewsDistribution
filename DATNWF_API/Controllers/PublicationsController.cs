using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using DATNWF_API.Models.DTO; // Might need DTOs if they existed, but returning Models directly as before is fine

namespace DATNWF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public PublicationsController(ThanhnienContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabBao>>> GetPublications()
        {
            if (_context.TabBaos == null)
            {
                return NotFound();
            }
            return await _context.TabBaos.ToListAsync();
        }

        [HttpGet("NgoaiLe")]
        public async Task<ActionResult<IEnumerable<TabBaoNgoaiLe>>> GetPublicationsNgoaiLe()
        {
            if (_context.TabBaoNgoaiLes == null)
            {
                return NotFound();
            }
            return await _context.TabBaoNgoaiLes.ToListAsync();
        }

        [HttpGet("BaoHomNay")]
        public async Task<ActionResult<IEnumerable<object>>> GetBaoHomNay()
        {
            if (_context.TabBaos == null)
            {
                return NotFound();
            }
            DayOfWeek today = DateTime.Now.DayOfWeek;
            
            var query = _context.TabBaos.AsQueryable();

            switch (today)
            {
                case DayOfWeek.Sunday: query = query.Where(b => b.Thu1 == true); break;
                case DayOfWeek.Monday: query = query.Where(b => b.Thu2 == true); break;
                case DayOfWeek.Tuesday: query = query.Where(b => b.Thu3 == true); break;
                case DayOfWeek.Wednesday: query = query.Where(b => b.Thu4 == true); break;
                case DayOfWeek.Thursday: query = query.Where(b => b.Thu5 == true); break;
                case DayOfWeek.Friday: query = query.Where(b => b.Thu6 == true); break;
                case DayOfWeek.Saturday: query = query.Where(b => b.Thu7 == true); break;
            }

            var result = await query.Select(b => new { b.MaBao, b.Ten }).ToListAsync();
            return Ok(result);
        }

        [HttpDelete("{maBao}")]
        public async Task<IActionResult> DeletePublication(string maBao)
        {
            if (_context.TabBaos == null)
            {
                return NotFound();
            }
            
            var tabBao = await _context.TabBaos.FindAsync(maBao);
            if (tabBao == null)
            {
                return NotFound();
            }

            // Delete NgoaiLe first
            var ngoaiLeList = await _context.TabBaoNgoaiLes.Where(n => n.MaBao == maBao).ToListAsync();
            _context.TabBaoNgoaiLes.RemoveRange(ngoaiLeList);

            // Delete Bao
            _context.TabBaos.Remove(tabBao);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Likely a foreign key constraint issue (e.g. 547)
                return Conflict("Cannot delete because of foreign key constraint.");
            }

            return NoContent();
        }
    }
}

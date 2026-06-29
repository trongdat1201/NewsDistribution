using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using DATNWF_API.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace DATNWF_API.Controllers
{
    [Authorize(Policy = "ReportOrAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public InventoriesController(ThanhnienContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TonDto>>> LayDanhSachTonKho()
        {
            if (_context.TabTons == null)
            {
                return NotFound("Không tìm thấy bảng Tồn kho.");
            }

            var danhSach = await _context.TabTons
                .Select(t => new TonDto
                {
                    Ngay = t.Ngay,
                    MaBao = t.MaBao,     
                    TenBao = t.TenBao,   
                    SoBao = t.SoBao,     
                    SlPhatHanh = t.SlPhatHanh,
                    Banthuc = t.Banthuc,
                    BanLe = t.BanLe,
                    DieuPhoi = t.DieuPhoi,
                    Ton = t.Ton
                })
                .OrderByDescending(t => t.Ngay)
                .ToListAsync();

            return Ok(danhSach);
        }
    }
}
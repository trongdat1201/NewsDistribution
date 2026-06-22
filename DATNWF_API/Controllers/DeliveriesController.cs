using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using DATNWF_API.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATNWF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public DeliveriesController(ThanhnienContext context)
        {
            _context = context;
        }

        // Lệnh GET: /api/Deliveries 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DieuPhoiDto>>> LayDanhSachDieuPhoi()
        {
            if (_context.TabDieuPhois == null)
            {
                return NotFound("Không tìm thấy bảng Điều Phối trong Database.");
            }

            var danhSach = await _context.TabDieuPhois
                .Select(dp => new DieuPhoiDto
                {
                    Sohd = dp.SoHd,           // Đã khớp với 'SoHd'
                    Makh = dp.Makh,
                    NgayLapPhieu = dp.Ngay,   // Đã khớp 'NgayLapPhieu' với cột 'Ngay'
                    TuNgay = dp.Tungay,       // Đã khớp với 'Tungay' (chữ n thường)
                    DenNgay = dp.Denngay,     // Đã khớp với 'Denngay' (chữ n thường)
                    GhiChu = dp.GhiChu
                })
                .ToListAsync();

            return Ok(danhSach);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using System.Linq;
using System.Threading.Tasks;
using DATNWF_API.Models.DTO;

using Microsoft.AspNetCore.Authorization;

namespace DATNWF_API.Controllers
{
    [Authorize(Policy = "StaffOrAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public DashboardController(ThanhnienContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboardData()
        {
            var summary = new DashboardSummaryDto();

            // 1. LẤY CÁC THÔNG SỐ TỔNG
            summary.TongDoanhThu = await _context.TabChitiethoadons.SumAsync(x => (decimal?)x.ThanhTien) ?? 0;
            summary.TongSoBao = await _context.TabChitiethoadons.SumAsync(x => (int?)(x.SoLuongThuc + x.SoLuongPhatSinh)) ?? 0;
            summary.TongKhachHang = await _context.TabKhachhangs.CountAsync();

            // 2. BIỂU ĐỒ TRÒN 
            summary.TyTrongDoanhThu = await (from ct in _context.TabChitiethoadons
                                             join b in _context.TabBaos on ct.MaBao equals b.MaBao
                                             group ct by b.Ten into g
                                             select new ThongKeTronDto
                                             {
                                                 TenBao = g.Key,
                                                 DoanhThu = g.Sum(x => (decimal?)x.ThanhTien) ?? 0
                                             }).ToListAsync();

            // 3. BIỂU ĐỒ ĐƯỜNG (Đã xóa chữ .Value)
            var timelineDb = await _context.TabChitiethoadons
                                .GroupBy(x => x.NgayNhan.Date)
                                .Select(g => new { Ngay = g.Key, Tong = g.Sum(x => (double?)x.ThanhTien) / 1000000 ?? 0 })
                                .OrderByDescending(x => x.Ngay).Take(30).ToListAsync();

            summary.BienDongDoanhThu = timelineDb.OrderBy(x => x.Ngay)
                                .Select(x => new ThongKeDuongDto { Ngay = x.Ngay.ToString("dd/MM"), DoanhThu = x.Tong }).ToList();

            // 4. BIỂU ĐỒ CỘT (Đã xóa chữ .Value)
            var inventoryDb = await _context.TabTons
                                .GroupBy(x => x.Ngay.Date)
                                .Select(g => new {
                                    Ngay = g.Key,
                                    PhatHanh = g.Sum(x => (double?)x.SlPhatHanh) ?? 0,
                                    TieuThu = g.Sum(x => (double?)(x.Banthuc + x.BanLe + x.DieuPhoi)) ?? 0,
                                    TonKho = g.Sum(x => (double?)x.Ton) ?? 0
                                }).OrderByDescending(x => x.Ngay).Take(7).ToListAsync();

            summary.ThongKeTonKho = inventoryDb.OrderBy(x => x.Ngay)
                                .Select(x => new ThongKeCotDto { Ngay = x.Ngay.ToString("dd/MM"), PhatHanh = x.PhatHanh, TieuThu = x.TieuThu, TonKho = x.TonKho }).ToList();

            // 5. BIỂU ĐỒ CỘT NGANG 
            summary.KhachHangTiemNang = await (from ct in _context.TabChitiethoadons
                                               join hd in _context.TabHoadons on ct.Sohd equals hd.Sohd
                                               join kh in _context.TabKhachhangs on hd.Makh equals kh.Makh
                                               group ct by kh.Ten into g
                                               select new TopKhachHangDto
                                               {
                                                   TenKhachHang = g.Key,
                                                   SoLuongMua = g.Sum(x => (double?)(x.SoLuongThuc + x.SoLuongPhatSinh)) ?? 0
                                               })
                                               .OrderBy(x => x.SoLuongMua).Take(10).ToListAsync();

            return Ok(summary);
        }
    }
}
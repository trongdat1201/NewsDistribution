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
    [Authorize(Policy = "StaffOrAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public DeliveriesController(ThanhnienContext context)
        {
            _context = context;
        }

        // GET: api/Deliveries 
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

        // GET: api/Deliveries/{sohd}/details
        [HttpGet("{sohd}/details")]
        public async Task<ActionResult> GetDeliveryDetails(string sohd)
        {
            var details = await _context.TabChiTietDieuPhois
                .Where(d => d.Sohd == sohd)
                .Select(d => new
                {
                    Sohd = d.Sohd,
                    NgayNhan = d.NgayNhan,
                    MaBao = d.MaBao,
                    TenBao = d.Tenbao,
                    SoBao = d.Sobao,
                    DonGia = d.DonGia,
                    SoluongDieuPhoi = d.SoluongDieuPhoi,
                    SoluongBan = d.SoluongBan,
                    ThanhTien = d.ThanhTien
                })
                .OrderBy(d => d.NgayNhan)
                .ToListAsync();
            return Ok(details);
        }

        // POST: api/Deliveries
        [HttpPost]
        public async Task<IActionResult> SaveDelivery([FromBody] DeliveryInputModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Sohd))
                return BadRequest("Dữ liệu không hợp lệ.");

            // Tìm master record hiện tại
            var dp = await _context.TabDieuPhois.FirstOrDefaultAsync(x => x.SoHd == model.Sohd);
            bool isNew = (dp == null);

            if (isNew)
            {
                dp = new TabDieuPhoi { SoHd = model.Sohd };
                _context.TabDieuPhois.Add(dp);
            }

            dp.Makh = model.Makh;
            dp.Ngay = model.Ngay;
            dp.Tungay = model.Tungay;
            dp.Denngay = model.Denngay;
            dp.GhiChu = model.GhiChu;

            // Xóa chi tiết cũ nếu có
            var existingDetails = await _context.TabChiTietDieuPhois.Where(d => d.Sohd == model.Sohd).ToListAsync();
            _context.TabChiTietDieuPhois.RemoveRange(existingDetails);

            // Thêm chi tiết mới
            foreach (var detail in model.Details)
            {
                var newDetail = new TabChiTietDieuPhoi
                {
                    Sohd = model.Sohd,
                    NgayNhan = detail.NgayNhan,
                    MaBao = detail.MaBao,
                    Tenbao = detail.TenBao,
                    Sobao = detail.SoBao,
                    DonGia = (decimal)detail.DonGia,
                    SoluongDieuPhoi = detail.SoluongDieuPhoi,
                    SoluongBan = detail.SoluongBan,
                    ThanhTien = (decimal)detail.ThanhTien
                };
                _context.TabChiTietDieuPhois.Add(newDetail);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // DELETE: api/Deliveries/{sohd}
        [HttpDelete("{sohd}")]
        public async Task<IActionResult> DeleteDelivery(string sohd)
        {
            var dp = await _context.TabDieuPhois.FirstOrDefaultAsync(x => x.SoHd == sohd);
            if (dp == null) return NotFound();

            var details = await _context.TabChiTietDieuPhois.Where(d => d.Sohd == sohd).ToListAsync();
            _context.TabChiTietDieuPhois.RemoveRange(details);
            _context.TabDieuPhois.Remove(dp);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class DeliveryInputModel
    {
        public string Sohd { get; set; } = null!;
        public string Makh { get; set; } = null!;
        public DateTime Ngay { get; set; }
        public DateTime Tungay { get; set; }
        public DateTime Denngay { get; set; }
        public string? GhiChu { get; set; }
        public List<DeliveryDetailInputModel> Details { get; set; } = new();
    }

    public class DeliveryDetailInputModel
    {
        public DateTime NgayNhan { get; set; }
        public string MaBao { get; set; } = null!;
        public string? TenBao { get; set; }
        public string? SoBao { get; set; }
        public double DonGia { get; set; }
        public int SoluongDieuPhoi { get; set; }
        public int SoluongBan { get; set; }
        public double ThanhTien { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;

using Microsoft.AspNetCore.Authorization;

namespace DATNWF_API.Controllers
{
    [Authorize(Policy = "ReportOrAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public InvoicesController(ThanhnienContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabHoadon>>> GetInvoices()
        {
            if (_context.TabHoadons == null)
            {
                return NotFound();
            }
            return await _context.TabHoadons
                .Select(h => new TabHoadon
                {
                    Sohd = h.Sohd,
                    Makh = h.Makh,
                    NgayLapPhieu = h.NgayLapPhieu,
                    TuNgay = h.TuNgay,
                    DenNgay = h.DenNgay,
                    Ghichu = h.Ghichu,
                    ThanhToan = h.ThanhToan
                })
                .ToListAsync();
        }

        // GET: api/Invoices/{sohd}/details
        [HttpGet("{sohd}/details")]
        public async Task<ActionResult> GetInvoiceDetails(string sohd)
        {
            var details = await _context.TabChitiethoadons
                .Where(d => d.Sohd == sohd)
                .Select(d => new
                {
                    Sohd = d.Sohd,
                    NgayNhan = d.NgayNhan,
                    MaBao = d.MaBao,
                    TenBao = d.TenBao,
                    SoBao = d.SoBao,
                    SoLuongThuc = d.SoLuongThuc,
                    SoLuongPhatSinh = d.SoLuongPhatSinh,
                    DonGia = d.DonGia,
                    ThanhTien = d.ThanhTien,
                    DieuPhoi = d.DieuPhoi
                })
                .OrderBy(d => d.NgayNhan)
                .ToListAsync();
            return Ok(details);
        }

        // POST: api/Invoices
        [HttpPost]
        public async Task<IActionResult> SaveInvoice([FromBody] InvoiceInputModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Sohd))
                return BadRequest("Dữ liệu hóa đơn không hợp lệ.");

            var hd = await _context.TabHoadons.FirstOrDefaultAsync(x => x.Sohd == model.Sohd);
            bool isNew = (hd == null);

            if (isNew)
            {
                hd = new TabHoadon { Sohd = model.Sohd };
                _context.TabHoadons.Add(hd);
            }

            hd.Makh = model.Makh;
            hd.NgayLapPhieu = model.NgayLapPhieu;
            hd.TuNgay = model.TuNgay;
            hd.DenNgay = model.DenNgay;
            hd.Ghichu = model.Ghichu;
            hd.ThanhToan = model.ThanhToan;

            // Xóa chi tiết hóa đơn cũ
            var existingDetails = await _context.TabChitiethoadons.Where(d => d.Sohd == model.Sohd).ToListAsync();
            _context.TabChitiethoadons.RemoveRange(existingDetails);

            // Thêm chi tiết hóa đơn mới
            foreach (var detail in model.Details)
            {
                var newDetail = new TabChitiethoadon
                {
                    Sohd = model.Sohd,
                    NgayNhan = detail.NgayNhan,
                    MaBao = detail.MaBao,
                    TenBao = detail.TenBao,
                    SoBao = detail.SoBao,
                    SoLuongThuc = detail.SoLuongThuc,
                    SoLuongPhatSinh = detail.SoLuongPhatSinh,
                    DonGia = detail.DonGia,
                    ThanhTien = detail.ThanhTien,
                    DieuPhoi = detail.DieuPhoi
                };
                _context.TabChitiethoadons.Add(newDetail);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // DELETE: api/Invoices/{sohd}
        [HttpDelete("{sohd}")]
        public async Task<IActionResult> DeleteInvoice(string sohd)
        {
            var hd = await _context.TabHoadons.FirstOrDefaultAsync(x => x.Sohd == sohd);
            if (hd == null) return NotFound();

            var details = await _context.TabChitiethoadons.Where(d => d.Sohd == sohd).ToListAsync();
            _context.TabChitiethoadons.RemoveRange(details);
            _context.TabHoadons.Remove(hd);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class InvoiceInputModel
    {
        public string Sohd { get; set; } = null!;
        public string Makh { get; set; } = null!;
        public DateTime NgayLapPhieu { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string? Ghichu { get; set; }
        public bool ThanhToan { get; set; }
        public List<InvoiceDetailInputModel> Details { get; set; } = new();
    }

    public class InvoiceDetailInputModel
    {
        public DateTime NgayNhan { get; set; }
        public string MaBao { get; set; } = null!;
        public string? TenBao { get; set; }
        public int? SoBao { get; set; }
        public int? SoLuongThuc { get; set; }
        public int? SoLuongPhatSinh { get; set; }
        public double? DonGia { get; set; }
        public double? ThanhTien { get; set; }
        public int? DieuPhoi { get; set; }
    }
}

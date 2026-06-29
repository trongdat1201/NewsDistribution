using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;

using Microsoft.AspNetCore.Authorization;

namespace DATNWF_API.Controllers
{
    [Authorize(Policy = "StaffOrAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public CustomersController(ThanhnienContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            var list = await _context.TabKhachhangs
                .Select(kh => new
                {
                    MaKH = kh.Makh,
                    Ten = kh.Ten,
                    DiaChi = kh.Diachi,
                    DienThoai = kh.Dienthoai,
                    ChietKhau = kh.Chietkhau,
                    PPh = kh.PPh,
                    PKt = kh.PKt,
                    Uutien = kh.Uutien
                }).ToListAsync();
            return Ok(list);
        }

        // GET: api/Customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomer(string id)
        {
            var kh = await _context.TabKhachhangs.FindAsync(id);
            if (kh == null) return NotFound();
            return Ok(new
            {
                MaKH = kh.Makh,
                Ten = kh.Ten,
                DiaChi = kh.Diachi,
                DienThoai = kh.Dienthoai,
                ChietKhau = kh.Chietkhau,
                PPh = kh.PPh,
                PKt = kh.PKt,
                Uutien = kh.Uutien
            });
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerInputModel model)
        {
            if (model == null) return BadRequest();

            var kh = new TabKhachhang
            {
                Makh = model.MaKH,
                Ten = model.Ten,
                Diachi = model.DiaChi,
                Dienthoai = model.DienThoai,
                Chietkhau = model.ChietKhau,
                PPh = model.PPh,
                PKt = model.PKt,
                Uutien = model.Uutien
            };

            _context.TabKhachhangs.Add(kh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await _context.TabKhachhangs.AnyAsync(e => e.Makh == kh.Makh))
                    return Conflict("Mã khách hàng đã tồn tại.");
                throw;
            }

            return CreatedAtAction(nameof(GetCustomer), new { id = kh.Makh }, model);
        }

        // PUT: api/Customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] CustomerInputModel model)
        {
            if (id != model.MaKH) return BadRequest("Mã khách hàng không trùng khớp.");

            var kh = await _context.TabKhachhangs.FindAsync(id);
            if (kh == null) return NotFound();

            kh.Ten = model.Ten;
            kh.Diachi = model.DiaChi;
            kh.Dienthoai = model.DienThoai;
            kh.Chietkhau = model.ChietKhau;
            kh.PPh = model.PPh;
            kh.PKt = model.PKt;
            kh.Uutien = model.Uutien;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var kh = await _context.TabKhachhangs.FindAsync(id);
            if (kh == null) return NotFound();

            // Kiểm tra ràng buộc khóa ngoại trước khi xóa
            bool checkHD = await _context.TabHoadons.AnyAsync(h => h.Makh == id);
            bool checkDP = await _context.TabDieuPhois.AnyAsync(d => d.Makh == id);
            if (checkHD || checkDP)
            {
                return Conflict("Không thể xóa khách hàng vì có hóa đơn hoặc điều phối liên quan.");
            }

            _context.TabKhachhangs.Remove(kh);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Customers/recent-orders
        [HttpGet("recent-orders")]
        public async Task<ActionResult> GetRecentOrders()
        {
            var recent = await (from kh in _context.TabKhachhangs
                                join hd in _context.TabHoadons on kh.Makh equals hd.Makh
                                group hd by new { kh.Makh, kh.Ten } into g
                                orderby g.Max(x => x.NgayLapPhieu) descending
                                select new
                                {
                                    MaKH = g.Key.Makh,
                                    Ten = g.Key.Ten
                                }).Take(10).ToListAsync();
            return Ok(recent);
        }

        // GET: api/Customers/classification-chart
        [HttpGet("classification-chart")]
        public async Task<ActionResult> GetClassificationChart()
        {
            var classification = await _context.TabKhachhangs
                .GroupBy(kh => kh.PPh && kh.PKt ? "P_PH & P_KT" : (kh.PPh ? "P_PH" : (kh.PKt ? "P_KT" : "Không phân loại")))
                .Select(g => new
                {
                    Loai = g.Key,
                    SoLuong = g.Count()
                }).ToListAsync();
            return Ok(classification);
        }

        // GET: api/Customers/top-revenue-chart
        [HttpGet("top-revenue-chart")]
        public async Task<ActionResult> GetTopRevenueChart()
        {
            var topCustomers = await (from kh in _context.TabKhachhangs
                                      join h in _context.TabHoadons on kh.Makh equals h.Makh
                                      join ct in _context.TabChitiethoadons on h.Sohd equals ct.Sohd
                                      group ct by new { kh.Makh, kh.Ten } into g
                                      select new
                                      {
                                          Ten = g.Key.Ten,
                                          TongDoanhThu = g.Sum(x => (double?)x.ThanhTien) ?? 0
                                      })
                                      .OrderByDescending(x => x.TongDoanhThu)
                                      .Take(5)
                                      .ToListAsync();
            return Ok(topCustomers);
        }

        // GET: api/Customers/{id}/growth-chart
        [HttpGet("{id}/growth-chart")]
        public async Task<ActionResult> GetCustomerGrowthChart(string id)
        {
            var points = await (from h in _context.TabHoadons
                                join ct in _context.TabChitiethoadons on h.Sohd equals ct.Sohd
                                where h.Makh == id
                                group ct by h.NgayLapPhieu.Year into g
                                orderby g.Key
                                select new
                                {
                                    Label = g.Key.ToString(),
                                    DoanhThu = g.Sum(x => (double?)x.ThanhTien) ?? 0,
                                    SoDonHang = g.Select(x => x.Sohd).Distinct().Count()
                                }).ToListAsync();
            return Ok(points);
        }

        // GET: api/Customers/{id}/transactions
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult> GetCustomerTransactions(string id)
        {
            var list = await (from h in _context.TabHoadons
                              join ct in _context.TabChitiethoadons on h.Sohd equals ct.Sohd
                              where h.Makh == id
                              orderby h.NgayLapPhieu descending, h.Sohd
                              select new
                              {
                                  SoHD = h.Sohd,
                                  NgayLapPhieu = h.NgayLapPhieu,
                                  TenBao = ct.TenBao,
                                  SoLuong = ct.SoLuongThuc,
                                  DonGia = ct.DonGia,
                                  ThanhTien = ct.ThanhTien
                              }).ToListAsync();
            return Ok(list);
        }
    }

    public class CustomerInputModel
    {
        public string MaKH { get; set; } = null!;
        public string Ten { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public short ChietKhau { get; set; }
        public bool PPh { get; set; }
        public bool PKt { get; set; }
        public string? Uutien { get; set; }
    }
}
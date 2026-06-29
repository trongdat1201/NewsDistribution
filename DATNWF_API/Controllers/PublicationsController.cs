using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace DATNWF_API.Controllers
{
    [Authorize(Policy = "StaffOrAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo("en-US");

        public PublicationsController(ThanhnienContext context)
        {
            _context = context;
        }

        // GET: api/Publications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TabBao>>> GetPublications()
        {
            if (_context.TabBaos == null)
            {
                return NotFound();
            }
            return await _context.TabBaos
                .Select(b => new TabBao
                {
                    MaBao = b.MaBao,
                    Ten = b.Ten,
                    DonGia = b.DonGia,
                    Dvt = b.Dvt,
                    NgayBatDau = b.NgayBatDau,
                    SoLanPhtrongTuan = b.SoLanPhtrongTuan,
                    Sogoc = b.Sogoc,
                    Thu1 = b.Thu1,
                    Thu2 = b.Thu2,
                    Thu3 = b.Thu3,
                    Thu4 = b.Thu4,
                    Thu5 = b.Thu5,
                    Thu6 = b.Thu6,
                    Thu7 = b.Thu7
                })
                .ToListAsync();
        }

        // GET: api/Publications/NgoaiLe
        [HttpGet("NgoaiLe")]
        public async Task<ActionResult<IEnumerable<TabBaoNgoaiLe>>> GetPublicationsNgoaiLe()
        {
            if (_context.TabBaoNgoaiLes == null)
            {
                return NotFound();
            }
            return await _context.TabBaoNgoaiLes
                .Select(n => new TabBaoNgoaiLe
                {
                    MaBao = n.MaBao,
                    NgayPhatHanh = n.NgayPhatHanh,
                    SoLanTrongNam = n.SoLanTrongNam
                })
                .ToListAsync();
        }

        // GET: api/Publications/BaoHomNay
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

        // GET: api/Publications/top-revenue
        [HttpGet("top-revenue")]
        public async Task<ActionResult> GetTopRevenue()
        {
            var top = await (from cthd in _context.TabChitiethoadons
                             join b in _context.TabBaos on cthd.MaBao equals b.MaBao
                             group cthd by b.Ten into g
                             select new
                             {
                                 TenBao = g.Key,
                                 TongDoanhThu = g.Sum(x => (double?)x.ThanhTien) ?? 0
                             })
                             .OrderByDescending(x => x.TongDoanhThu)
                             .ToListAsync();
            return Ok(top);
        }

        // POST: api/Publications
        [HttpPost]
        public async Task<IActionResult> SavePublication([FromBody] PublicationInputModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.MaBao))
            {
                return BadRequest("Dữ liệu báo không hợp lệ.");
            }

            var tabBao = await _context.TabBaos.FirstOrDefaultAsync(b => b.MaBao == model.MaBao);
            bool isNew = (tabBao == null);

            if (isNew)
            {
                tabBao = new TabBao { MaBao = model.MaBao };
                _context.TabBaos.Add(tabBao);
            }

            tabBao.Ten = model.Ten;
            tabBao.Dvt = model.Dvt;
            tabBao.DonGia = model.DonGia;
            tabBao.SoLanPhtrongTuan = model.SoLanPhtrongTuan;
            tabBao.Sogoc = model.Sogoc;
            tabBao.NgayBatDau = model.NgayBatDau;
            tabBao.Thu1 = model.Thu1;
            tabBao.Thu2 = model.Thu2;
            tabBao.Thu3 = model.Thu3;
            tabBao.Thu4 = model.Thu4;
            tabBao.Thu5 = model.Thu5;
            tabBao.Thu6 = model.Thu6;
            tabBao.Thu7 = model.Thu7;

            // Xóa ngoại lệ cũ nếu có
            var existingNgoaiLe = await _context.TabBaoNgoaiLes.Where(n => n.MaBao == model.MaBao).ToListAsync();
            _context.TabBaoNgoaiLes.RemoveRange(existingNgoaiLe);

            // Thêm ngoại lệ mới
            foreach (var nl in model.NgoaiLeList)
            {
                var newNl = new TabBaoNgoaiLe
                {
                    MaBao = model.MaBao,
                    NgayPhatHanh = nl.NgayPhatHanh,
                    SoLanTrongNam = nl.SoLanTrongNam ?? 1
                };
                _context.TabBaoNgoaiLes.Add(newNl);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // DELETE: api/Publications/{maBao}
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

            // Xóa NgoaiLe liên quan trước
            var ngoaiLeList = await _context.TabBaoNgoaiLes.Where(n => n.MaBao == maBao).ToListAsync();
            _context.TabBaoNgoaiLes.RemoveRange(ngoaiLeList);

            // Xóa đầu báo
            _context.TabBaos.Remove(tabBao);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Không thể xóa báo vì đang có dữ liệu liên quan trong kho hoặc hóa đơn điều phối!");
            }

            return NoContent();
        }
    }

    public class PublicationInputModel
    {
        public string MaBao { get; set; } = null!;
        public string Ten { get; set; } = null!;
        public string? Dvt { get; set; }
        public double DonGia { get; set; }
        public int SoLanPhtrongTuan { get; set; }
        public int Sogoc { get; set; }
        public DateTime NgayBatDau { get; set; }
        public bool Thu1 { get; set; }
        public bool Thu2 { get; set; }
        public bool Thu3 { get; set; }
        public bool Thu4 { get; set; }
        public bool Thu5 { get; set; }
        public bool Thu6 { get; set; }
        public bool Thu7 { get; set; }
        public List<PublicationNgoaiLeInputModel> NgoaiLeList { get; set; } = new();
    }

    public class PublicationNgoaiLeInputModel
    {
        public DateTime NgayPhatHanh { get; set; }
        public int? SoLanTrongNam { get; set; }
    }
}

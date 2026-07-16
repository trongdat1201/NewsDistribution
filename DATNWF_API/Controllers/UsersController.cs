using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATNWF_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace DATNWF_API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ThanhnienContext _context;

        public UsersController(ThanhnienContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var list = await _context.TabLogins
                .Select(u => new
                {
                    TenDangNhap = u.TenDangNhap,
                    Role = u.Role
                }).ToListAsync();
            return Ok(list);
        }

        // GET: api/Users/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<object>> GetUser(string username)
        {
            var u = await _context.TabLogins.FirstOrDefaultAsync(x => x.TenDangNhap == username);
            if (u == null) return NotFound();

            return Ok(new
            {
                TenDangNhap = u.TenDangNhap,
                Role = u.Role
            });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserInputModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.TenDangNhap) || string.IsNullOrWhiteSpace(model.MatKhau))
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }

            if (await _context.TabLogins.AnyAsync(x => x.TenDangNhap == model.TenDangNhap))
            {
                return Conflict("Tên đăng nhập đã tồn tại.");
            }

            var user = new TabLogin
            {
                TenDangNhap = model.TenDangNhap,
                // Hashing password with BCrypt (which generates dynamic salt internally)
                MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                Role = model.Role
            };

            _context.TabLogins.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { username = user.TenDangNhap }, new {
                TenDangNhap = user.TenDangNhap,
                Role = user.Role
            });
        }

        // PUT: api/Users/{username}
        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UserInputModel model)
        {
            if (username != model.TenDangNhap)
            {
                return BadRequest("Tên đăng nhập không trùng khớp.");
            }

            var user = await _context.TabLogins.FirstOrDefaultAsync(x => x.TenDangNhap == username);
            if (user == null) return NotFound();

            user.Role = model.Role;

            if (!string.IsNullOrWhiteSpace(model.MatKhau))
            {
                // Update password with new BCrypt hash
                user.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Users/{username}
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _context.TabLogins.FirstOrDefaultAsync(x => x.TenDangNhap == username);
            if (user == null) return NotFound();

            _context.TabLogins.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class UserInputModel
    {
        public string TenDangNhap { get; set; } = null!;
        public string? MatKhau { get; set; }
        public string Role { get; set; } = null!;
    }
}

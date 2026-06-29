using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DATNWF_API.Models;

namespace DATNWF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ThanhnienContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ThanhnienContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Vui lòng cung cấp đầy đủ thông tin đăng nhập.");
            }

            var user = await _context.TabLogins
                .FirstOrDefaultAsync(u => u.TenDangNhap == request.Username);

            if (user == null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            bool isPasswordCorrect = false;

            // Kiểm tra mật khẩu băm BCrypt (thường bắt đầu bằng $2a$, $2b$ hoặc $2y$)
            if (user.MatKhau.StartsWith("$2a$") || user.MatKhau.StartsWith("$2b$") || user.MatKhau.StartsWith("$2y$"))
            {
                try
                {
                    isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.MatKhau);
                }
                catch
                {
                    isPasswordCorrect = false;
                }
            }
            else
            {
                // Fallback nếu mật khẩu cũ chưa được băm trong database
                isPasswordCorrect = (request.Password == user.MatKhau);
                if (isPasswordCorrect)
                {
                    // Tự động nâng cấp mật khẩu sang BCrypt
                    user.MatKhau = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    await _context.SaveChangesAsync();
                }
            }

            if (!isPasswordCorrect)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            // Tạo Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim("Ht", user.Ht.ToString().ToLower()),
                new Claim("Nv", user.Nv.ToString().ToLower()),
                new Claim("Bc", user.Bc.ToString().ToLower())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new LoginResponse
            {
                Token = tokenString,
                Username = user.TenDangNhap,
                Ht = user.Ht,
                Nv = user.Nv,
                Bc = user.Bc
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool Ht { get; set; }
        public bool Nv { get; set; }
        public bool Bc { get; set; }
    }
}

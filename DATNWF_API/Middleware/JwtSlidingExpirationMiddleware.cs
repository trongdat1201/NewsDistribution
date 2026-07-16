using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DATNWF_API.Middleware
{
    public class JwtSlidingExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtSlidingExpirationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Cho phép request chạy tiếp qua pipeline
            await _next(context);

            // Kiểm tra xem người dùng đã được xác thực hợp lệ hay chưa
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                string authHeader = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string tokenString = authHeader.Substring(7).Trim();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    
                    try
                    {
                        var jwtToken = tokenHandler.ReadJwtToken(tokenString);
                        var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                        
                        if (expClaim != null)
                        {
                            var expTimeUnix = long.Parse(expClaim.Value);
                            var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expTimeUnix).UtcDateTime;
                            
                            // Tính toán thời gian sống của Token
                            var totalLifespan = expDateTime - jwtToken.IssuedAt;
                            var remainingTime = expDateTime - DateTime.UtcNow;

                            // Nếu token đã đi qua hơn một nửa thời gian sống, tự động gia hạn Token mới
                            if (remainingTime.TotalMinutes > 0 && remainingTime.TotalMinutes < (totalLifespan.TotalMinutes / 2))
                            {
                                var jwtSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
                                var key = Encoding.ASCII.GetBytes(jwtSecret);

                                var claims = context.User.Claims.ToList();
                                // Loại bỏ các claims do hệ thống tự sinh để tránh bị lặp khi ghi đè
                                var claimsToKeep = claims.Where(c => c.Type != "exp" && c.Type != "nbf" && c.Type != "iat" && c.Type != "iss" && c.Type != "aud").ToList();

                                var tokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = new ClaimsIdentity(claimsToKeep),
                                    Expires = DateTime.UtcNow.AddMinutes(2), // Gia hạn thêm 2 phút để demo
                                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                                };

                                var newToken = tokenHandler.CreateToken(tokenDescriptor);
                                var newTokenString = tokenHandler.WriteToken(newToken);

                                // Trả về token mới qua Header phản hồi
                                context.Response.Headers.Append("X-New-Token", newTokenString);
                                context.Response.Headers.Append("Access-Control-Expose-Headers", "X-New-Token");
                            }
                        }
                    }
                    catch
                    {
                        // Bỏ qua nếu token không đọc được hoặc lỗi định dạng
                    }
                }
            }
        }
    }
}

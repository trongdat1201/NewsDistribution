using System;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ĐĂNG KÝ BỘ NÃO DATABASE VÀO HỆ THỐNG
    builder.Services.AddDbContext<DATNWF_API.Models.ThanhnienContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    // CẤU HÌNH JWT AUTHENTICATION
    var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
    var key = System.Text.Encoding.ASCII.GetBytes(jwtSecret);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Ht", "true"));
        options.AddPolicy("StaffOrAdmin", policy => policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Type == "Nv" || c.Type == "Ht") && c.Value.ToLower() == "true")
        ));
        options.AddPolicy("ReportOrAdmin", policy => policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Type == "Bc" || c.Type == "Ht") && c.Value.ToLower() == "true")
        ));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // BẮT LỖI VÀ IN RA MÀN HÌNH ĐỂ TÌM THỦ PHẠM
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("=== SERVER BỊ SẬP NGUỒN TỨC TƯỞI ===");
    Console.WriteLine($"LỖI CHÍNH: {ex.Message}");

    if (ex.InnerException != null)
    {
        Console.WriteLine($"CHI TIẾT SÂU HƠN: {ex.InnerException.Message}");
    }

    Console.WriteLine("====================================");
    Console.ResetColor();
    Console.WriteLine("Hãy chụp ảnh màn hình này lại. Bấm Enter để thoát...");
    Console.ReadLine(); // Lệnh này giúp giữ màn hình đen không bị văng mất
}
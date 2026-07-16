using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Serilog;
using DATNWF_API.Filters;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // CẤU HÌNH SERILOG GHI LOG AUDIT TRAIL
    var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }
    
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(
            Path.Combine(logDirectory, "audit-.txt"),
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        )
        .CreateLogger();

    builder.Host.UseSerilog();

    // ĐĂNG KÝ BỘ NÃO DATABASE VÀO HỆ THỐNG
    builder.Services.AddDbContext<DATNWF_API.Models.ThanhnienContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add services to the container. Register AuditLogFilter globally.
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<AuditLogFilter>();
    });
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
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("ROLE_HT"));
        options.AddPolicy("StaffOrAdmin", policy => policy.RequireRole("ROLE_HT", "ROLE_NV_PH"));
        options.AddPolicy("ReportOrAdmin", policy => policy.RequireRole("ROLE_HT", "ROLE_NV_KT"));
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
    
    // ĐĂNG KÝ MIDDLEWARE GIA HẠN TOKEN TRƯỢT (SLIDING EXPIRATION)
    app.UseMiddleware<DATNWF_API.Middleware.JwtSlidingExpirationMiddleware>();

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
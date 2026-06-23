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

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
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
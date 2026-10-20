using FilmMVC.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Отримуємо рядок підключення з файлу конфігурації
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Додаємо контекст FilmContext як сервіс у програму
builder.Services.AddDbContext<FilmContext>(options => options.UseSqlite(connection));

// Додаємо сервіси Razor Pages замість MVC
builder.Services.AddRazorPages();

var app = builder.Build();

// Обробка помилок у продакшні (за бажанням)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Обробляє запити до статичних файлів у папці wwwroot

app.UseRouting(); // Включаємо маршрутизацію

app.UseAuthorization();

// Підключаємо маршрутизацію для Razor Pages
app.MapRazorPages();

app.Run();
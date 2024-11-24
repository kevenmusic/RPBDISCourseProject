using MarriageAgency.Data;
using MarriageAgency.DataLayer.Data;
using MarriageAgency.Middleware;
using MarriageAgency.Models;
using MarriageAgency.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<MarriageAgencyContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("MarriageAgency")));
string connectionUsers = builder.Configuration.GetConnectionString("IdentityConnection");
// Настройка контекста для Identity (пользователи, роли, аутентификация и т.д.)
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionUsers));

// Настройка Identity с использованием ApplicationDbContext для аутентификации
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Добавление RazorPages и MVC
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Добавление сервисов для Identity
builder.Services.AddTransient<IServiceService, ServiceService>();

// Настройка кэширования
builder.Services.AddMemoryCache();

// Настройка сессий
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


// Использование MVC
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
//Использование RazorPages
builder.Services.AddRazorPages();
WebApplication app = builder.Build();

// Настройка HTTP конвейера
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
// Добавляем поддержку сессий
app.UseSession();

// Добавляем компонент middleware для кэширования
app.UseOperatinCache("Services 10");
// добавляем компонента miidleware по инициализации базы данных
app.UseDbInitializer();
// Маршрутизация
app.UseRouting();

// Использование Identity
app.UseAuthentication();
app.UseAuthorization();

// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

using BilsoftAnketPlatformu.Data;
using BilsoftAnketPlatformu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC ve Razor Pages servisleri
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Connection string'i appsettings.Development.json içinden alıyoruz
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// MySQL bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Identity ayarları
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    // Şifre kuralları
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Kullanıcı ayarları
    options.User.RequireUniqueEmail = true;

    // Giriş deneme kilitleme ayarları
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Login yönlendirme ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Aktif/pasif değişikliği açık oturumlara hemen yansısın.
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

var app = builder.Build();

// Hata yönetimi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Önce authentication, sonra authorization
app.UseAuthentication();
app.UseAuthorization();

// Varsayılan route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

using Data.Repos;
using Data.UnitOfWork;
using FinanceApp.Business.Services;
using FinanceApp.Business.Services.Concrete;
using FinanceApp.Business.Services.Interfaces;
using FinanceApp.Common;
using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<FinanceAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// IHttpContextAccessor ekleyelim
builder.Services.AddHttpContextAccessor();

// TenantProvider ekleyelim
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

// UnitOfWork ve Repository bağımlılıklarını ekleyelim
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Servisler
builder.Services.AddScoped<IAgreementService, AgreementService>();
builder.Services.AddScoped<IAgreementKeysService, AgreementKeysService>();
builder.Services.AddScoped<IJobsService, JobService>();
builder.Services.AddScoped<IPartnersService, BusinessPartnerService>();
builder.Services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi 30 dakika
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Veritabanı bağlantısını kontrol et ve oluştur
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceAppContext>();
    dbContext.Database.Migrate();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

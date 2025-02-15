using Data.Repos;
using Data.UnitOfWork;
using FinanceApp.Business.Services;
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

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Veritabanı bağlantısını kontrol et ve oluştur
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceAppContext>();
    dbContext.Database.Migrate();
}

// Middleware ekleyelim
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("TenantId"))
    {
        context.Request.Headers["TenantId"] = "1"; // Varsayılan Tenant ID
    }
    await next.Invoke();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

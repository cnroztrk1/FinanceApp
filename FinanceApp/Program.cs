using Data.Repos;
using Data.UnitOfWork;
using FinanceApp.Business.Services;
using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DbContext
builder.Services.AddDbContext<FinanceAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Data Katmaný
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

//Servislerin eklenmesi
builder.Services.AddScoped<IAgreementService, AgreementService>();
builder.Services.AddScoped<IAgreementKeysService, AgreementKeysService>();
builder.Services.AddScoped<IJobsService, JobService>();
builder.Services.AddScoped<IPartnersService, BusinessPartnerService>();
builder.Services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Uygulama baþlangýcýnda veritabanýnýn varlýðýný kontrol edip oluþturmak için scope oluþturuyoruz
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceAppContext>();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

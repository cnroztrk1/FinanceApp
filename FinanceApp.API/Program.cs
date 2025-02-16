using Data.Repos;
using Data.UnitOfWork;
using FinanceApp.API.Hubs;
using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS Politikasý Tanýmla
var corsPolicyName = "AllowFinanceApp";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        builder => builder
            .WithOrigins("https://localhost:5001") // FinanceApp'in çalýþtýðý port
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // SignalR için gerekli
});

// DbContext
builder.Services.AddDbContext<FinanceAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IJobsService, JobService>();
builder.Services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();
builder.Services.AddScoped<IPartnersService, BusinessPartnerService>();

// SignalR ve CORS Eklentileri
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinanceApp API", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

// CORS Kullan
app.UseCors(corsPolicyName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanceApp API v1");
    });
}

app.UseRouting();
app.UseAuthorization();

// SignalR için CORS'i aktif et
app.UseCors(corsPolicyName);
app.MapHub<RiskNotificationHub>("/riskhub").RequireCors(corsPolicyName);

app.MapControllers();

app.Run();

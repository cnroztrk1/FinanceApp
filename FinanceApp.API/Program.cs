using Data.Repos;
using Data.UnitOfWork;
using FinanceApp.API.Hubs;
using FinanceApp.API.Middleware;
using FinanceApp.Business.Services;
using FinanceApp.Business.Services.Concrete;
using FinanceApp.Business.Services.Interfaces;
using FinanceApp.Common;
using FinanceApp.Data;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddDistributedMemoryCache(); // Session için gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor(); // Session'ý kullanmak için gerekli
// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IJobsService, JobService>();
builder.Services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();
builder.Services.AddScoped<IPartnersService, BusinessPartnerService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAgreementService, AgreementService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
// SignalR ve CORS Eklentileri
builder.Services.AddSignalR();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FinanceApp API", Version = "v1" });

    // UserName ve Password Header Tanýmlamalarý
    c.AddSecurityDefinition("UserName", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "UserName",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "API istekleri için kullanýcý adýnýzý buraya girin."
    });

    c.AddSecurityDefinition("Password", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Password",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "API istekleri için þifrenizi buraya girin."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "UserName"
                }
            },
            new string[] {}
        },
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Password"
                }
            },
            new string[] {}
        }
    });
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
app.UseSession();
app.UseMiddleware<AuthenticationMiddleware>();
// SignalR için CORS'i aktif et
app.UseCors(corsPolicyName);
app.MapHub<RiskNotificationHub>("/riskhub").RequireCors(corsPolicyName);

app.MapControllers();

app.Run();

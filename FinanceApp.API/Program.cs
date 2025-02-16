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

// CORS Politikas� Tan�mla
var corsPolicyName = "AllowFinanceApp";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        builder => builder
            .WithOrigins("https://localhost:5001") // FinanceApp'in �al��t��� port
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // SignalR i�in gerekli
});

// DbContext
builder.Services.AddDbContext<FinanceAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache(); // Session i�in gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum s�resi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor(); // Session'� kullanmak i�in gerekli
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

    // UserName ve Password Header Tan�mlamalar�
    c.AddSecurityDefinition("UserName", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "UserName",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "API istekleri i�in kullan�c� ad�n�z� buraya girin."
    });

    c.AddSecurityDefinition("Password", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Password",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "API istekleri i�in �ifrenizi buraya girin."
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
// SignalR i�in CORS'i aktif et
app.UseCors(corsPolicyName);
app.MapHub<RiskNotificationHub>("/riskhub").RequireCors(corsPolicyName);

app.MapControllers();

app.Run();

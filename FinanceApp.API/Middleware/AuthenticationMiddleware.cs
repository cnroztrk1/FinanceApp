using FinanceApp.Business.Services.Interfaces;
using System;
using System.Linq;

namespace FinanceApp.API.Middleware
{
    public class AuthenticationMiddleware //İsteklere auth ekliyoruz web soket hariç
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context, ILoginService loginService)
        {
            try
            {
                // SignalR bağlantıları için kimlik doğrulamasını atla
                if (context.Request.Path.StartsWithSegments("/riskhub"))
                {
                    await _next(context);
                    return;
                }

                if (!context.Request.Headers.ContainsKey("UserName") || !context.Request.Headers.ContainsKey("Password"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Kullanıcı adı ve şifre gerekli!");
                    return;
                }

                string username = context.Request.Headers["UserName"];
                string password = context.Request.Headers["Password"];

                var company = await loginService.Authenticate(username, password);
                if (company == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Geçersiz kullanıcı adı veya şifre.");
                    return;
                }

                // TenantId'yi HttpContext'e ekliyoruz.
                context.Items["TenantId"] = company.Id;

                _httpContextAccessor.HttpContext.Session.SetInt32("TenantId", company.Id);
                _httpContextAccessor.HttpContext.Session.SetString("UserName", company.UserName);
                // İşleme devam et
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuthenticationMiddleware hata aldı: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal Server Error");
            }
        }
    }
}

using FinanceApp.Business.Services.Interfaces;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Presentation.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(ILoginService loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model) //Login Companies içerisindeki data ile uyuşursa giriş yapar
        {
            if (ModelState.IsValid)
            {
                var company = await _loginService.Authenticate(model.UserName, model.Password);
                if (company != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("TenantId");
            _httpContextAccessor.HttpContext.Response.Headers["TenantId"] = string.Empty;
            _httpContextAccessor.HttpContext.Response.Headers["UserName"] = string.Empty;
            return RedirectToAction("Index");
        }
    }
}

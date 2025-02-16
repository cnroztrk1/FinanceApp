using FinanceApp.Business.Services;
using FinanceApp.Business.Services.Interfaces;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = await _loginService.Authenticate(model.UserName, model.Password);
                if (company != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("TenantId");
            _httpContextAccessor.HttpContext.Response.Headers["TenantId"] = "";
            _httpContextAccessor.HttpContext.Response.Headers["UserName"] = "";
            return RedirectToAction("Index");
        }
    }
}

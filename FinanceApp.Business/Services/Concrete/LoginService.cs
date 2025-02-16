using Data.UnitOfWork;
using FinanceApp.Business.Services.Interfaces;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace FinanceApp.Business.Services.Concrete
{
    public class LoginService : ILoginService //Login servis tenantlar için serviste ve arayüzde buradan ayırıyoruz. Giriş yapılan veya auth edilen kullanıcı burada sessiona setleniyor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Company> Authenticate(string username, string password)
        {
            var companies = await _unitOfWork.Companies.GetAllAsyncNoTenant();
            var hashedPassword = Company.HashPassword(password);

            var company = companies.FirstOrDefault(c => c.UserName == username && c.Password == hashedPassword);
            if (company != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("TenantId", company.Id.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,  // HTTPS kullanımına göre değiştirilebilir
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                _httpContextAccessor.HttpContext.Response.Cookies.Append("UserName", company.UserName, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

            }

            return company;
        }
        public async Task<Company> AuthenticateForService(string username, string password)
        {
            var companies = await _unitOfWork.Companies.GetAllAsyncNoTenant();
            var hashedPassword = Company.HashPassword(password);

            var company = companies.FirstOrDefault(c => c.UserName == username && c.Password == hashedPassword);

            return company;
        }
    }
}

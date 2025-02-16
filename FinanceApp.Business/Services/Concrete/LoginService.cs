using Data.UnitOfWork;
using FinanceApp.Business.Services.Interfaces;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace FinanceApp.Business.Services.Concrete
{
    public class LoginService : ILoginService
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
                _httpContextAccessor.HttpContext.Session.SetInt32("TenantId", company.Id);
                _httpContextAccessor.HttpContext.Session.SetString("UserName", company.UserName);
                _httpContextAccessor.HttpContext.Response.Headers["TenantId"] = company.Id.ToString();
                _httpContextAccessor.HttpContext.Response.Headers["UserName"] = company.UserName.ToString();
            }

            return company;
        }
    }
}

using FinanceApp.Data.Entities;
using System;
using System.Linq;

namespace FinanceApp.Business.Services.Interfaces
{
    public interface ILoginService
    {
        Task<Company> Authenticate(string username, string password);
    }
}

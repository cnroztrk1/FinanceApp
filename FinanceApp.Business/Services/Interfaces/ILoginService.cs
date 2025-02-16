using FinanceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services.Interfaces
{
    public interface ILoginService
    {
        Task<Company> Authenticate(string username, string password);
    }
}

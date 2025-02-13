using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public interface IAgreementService
    {
        Task<IEnumerable<Agreement>> GetAllAgreementsAsync();
        Task<Agreement> GetAgreementByIdAsync(int id);
        Task CreateAgreementAsync(Agreement agreement);
        Task UpdateAgreementAsync(Agreement agreement);
        Task DeleteAgreementAsync(int id);
    }
}

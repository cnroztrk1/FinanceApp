using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public interface IAgreementKeysService
    {
        Task<IEnumerable<AgreementKeys>> GetAllKeywordsAsync();
        Task<AgreementKeys> GetKeywordByIdAsync(int id);
        Task CreateKeywordAsync(AgreementKeys keyword);
        Task UpdateKeywordAsync(AgreementKeys keyword);
        Task DeleteKeywordAsync(int id);
    }
}

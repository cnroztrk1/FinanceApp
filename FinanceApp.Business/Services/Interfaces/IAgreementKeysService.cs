using FinanceApp.Data.Entities;

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

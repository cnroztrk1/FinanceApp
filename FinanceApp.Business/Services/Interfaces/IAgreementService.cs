using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public interface IAgreementService
    {
        Task<IEnumerable<Agreement>> GetAllAgreementsAsync();

        Task<IEnumerable<Agreement>> GetAgreementByTenantId(int id);
        Task<Agreement> GetAgreementByIdAsync(int id);
        Task CreateAgreementAsync(Agreement agreement);
        Task UpdateAgreementAsync(Agreement agreement);
        Task DeleteAgreementAsync(int id);
    }
}

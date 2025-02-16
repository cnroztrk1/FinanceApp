using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public interface IPartnersService
    {
        Task<IEnumerable<Partners>> GetAllPartnersAsync();
        Task<IEnumerable<Partners>> GetPartnerByTenantId(int id);
        Task<Partners> GetPartnerByIdAsync(int id);
        Task CreatePartnerAsync(Partners partner);
        Task UpdatePartnerAsync(Partners partner);
        Task DeletePartnerAsync(int id);
    }
}

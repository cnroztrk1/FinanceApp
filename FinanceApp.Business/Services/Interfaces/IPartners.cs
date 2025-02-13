using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public interface IPartnersService
    {
        Task<IEnumerable<Partners>> GetAllPartnersAsync();
        Task<Partners> GetPartnerByIdAsync(int id);
        Task CreatePartnerAsync(Partners partner);
        Task UpdatePartnerAsync(Partners partner);
        Task DeletePartnerAsync(int id);
    }
}

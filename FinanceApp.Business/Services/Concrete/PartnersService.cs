using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using System.Linq;

namespace FinanceApp.Business.Services
{
    public class BusinessPartnerService : IPartnersService //İş ortakları için Create Read Update Delete (CRUD) işlemleri
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _tenantId;

        public BusinessPartnerService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IEnumerable<Partners>> GetAllPartnersAsync()
        {
            return (await _unitOfWork.Partners.GetAllAsync()).Where(p => p.TenantId == _tenantId);
        }

        public async Task<Partners> GetPartnerByIdAsync(int id)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(id);
            return partner?.TenantId == _tenantId ? partner : null;
        }

        public async Task CreatePartnerAsync(Partners partner)
        {
            partner.TenantId = _tenantId;
            await _unitOfWork.Partners.AddAsync(partner);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePartnerAsync(Partners partner)
        {
            if (partner.TenantId == _tenantId)
            {
                _unitOfWork.Partners.Update(partner);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeletePartnerAsync(int id)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(id);
            if (partner != null && partner.TenantId == _tenantId)
            {
                _unitOfWork.Partners.Remove(partner);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}

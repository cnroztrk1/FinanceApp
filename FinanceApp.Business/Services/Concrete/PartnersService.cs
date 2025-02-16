using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public class BusinessPartnerService : IPartnersService // İş ortakları için Create Read Update Delete (CRUD) işlemleri
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly int _tenantId;
        private readonly string _cacheKey = "partners_{0}"; // Tenant bazlı cache key

        public BusinessPartnerService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Partners>> GetAllPartnersAsync()
        {
            string cacheKey = string.Format(_cacheKey, _tenantId);
            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                return (await _unitOfWork.Partners.GetAllAsync()).Where(p => p.TenantId == _tenantId);
            }, TimeSpan.FromMinutes(30)); // 30 dakika cache'de tut
        }

        public async Task<Partners> GetPartnerByIdAsync(int id)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(id);
            return partner?.TenantId == _tenantId ? partner : null;
        }

        public async Task<IEnumerable<Partners>> GetPartnerByTenantId(int id)
        {
            return (await _unitOfWork.Partners.GetAllAsyncNoTenant()).Where(p => p.TenantId == id);
        }

        public async Task CreatePartnerAsync(Partners partner)
        {
            partner.TenantId = _tenantId;
            await _unitOfWork.Partners.AddAsync(partner);
            await _unitOfWork.CompleteAsync();
            _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Cache temizleme
        }

        public async Task UpdatePartnerAsync(Partners partner)
        {
            if (partner.TenantId == _tenantId)
            {
                _unitOfWork.Partners.Update(partner);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Güncelleme sonrası cache temizleme
            }
        }

        public async Task DeletePartnerAsync(int id)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(id);
            if (partner != null && partner.TenantId == _tenantId)
            {
                _unitOfWork.Partners.Remove(partner);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Silme sonrası cache temizleme
            }
        }
    }
}

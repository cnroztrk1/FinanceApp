using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public class AgreementService : IAgreementService // Anlaşmalalar için CRUD işlemleri
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly int _tenantId;
        private readonly string _cacheKey = "agreements_{0}"; // Tenant bazlı cache key

        public AgreementService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Agreement>> GetAllAgreementsAsync()
        {
            string cacheKey = string.Format(_cacheKey, _tenantId);
            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                return (await _unitOfWork.Agreements.GetAllAsync()).Where(a => a.TenantId == _tenantId);
            }, TimeSpan.FromMinutes(30)); // Cache süresi 30 dakika
        }

        public async Task<IEnumerable<Agreement>> GetAgreementByTenantId(int id)
        {
            return (await _unitOfWork.Agreements.GetAllAsyncNoTenant()).Where(p => p.TenantId == id);
        }

        public async Task<Agreement> GetAgreementByIdAsync(int id)
        {
            var agreement = await _unitOfWork.Agreements.GetByIdAsync(id);
            return agreement?.TenantId == _tenantId ? agreement : null;
        }

        public async Task CreateAgreementAsync(Agreement agreement)
        {
            agreement.TenantId = _tenantId;
            await _unitOfWork.Agreements.AddAsync(agreement);
            await _unitOfWork.CompleteAsync();
            _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Cache temizleme
        }

        public async Task UpdateAgreementAsync(Agreement agreement)
        {
            if (agreement.TenantId == _tenantId)
            {
                _unitOfWork.Agreements.Update(agreement);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Güncelleme sonrası cache temizleme
            }
        }

        public async Task DeleteAgreementAsync(int id)
        {
            var agreement = await _unitOfWork.Agreements.GetByIdAsync(id);
            if (agreement != null && agreement.TenantId == _tenantId)
            {
                _unitOfWork.Agreements.Remove(agreement);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Silme sonrası cache temizleme
            }
        }
    }
}

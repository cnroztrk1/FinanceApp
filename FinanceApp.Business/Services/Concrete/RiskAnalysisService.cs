using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;

namespace FinanceApp.Business.Services
{
    public class RiskAnalysisService : IRiskAnalysisService // Risk analizi için Create Read Update Delete (CRUD) işlemleri
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly int _tenantId;
        private readonly string _cacheKey = "risk_analysis_{0}"; // Tenant bazlı cache key

        public RiskAnalysisService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<RiskAnalysis>> GetAllRiskAnalysesAsync()
        {
            string cacheKey = string.Format(_cacheKey, _tenantId);
            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                return (await _unitOfWork.RiskAnalysis.GetAllAsync()).Where(ra => ra.TenantId == _tenantId);
            }, TimeSpan.FromMinutes(30)); // 30 dakika cache'de tut
        }

        public async Task<RiskAnalysis> GetRiskAnalysisByIdAsync(int id)
        {
            var riskAnalysis = await _unitOfWork.RiskAnalysis.GetByIdAsync(id);
            return riskAnalysis?.TenantId == _tenantId ? riskAnalysis : null;
        }

        public async Task CreateRiskAnalysisAsync(RiskAnalysis riskAnalysis)
        {
            riskAnalysis.TenantId = _tenantId;
            await _unitOfWork.RiskAnalysis.AddAsync(riskAnalysis);
            await _unitOfWork.CompleteAsync();
            _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Cache temizleme
        }

        public async Task UpdateRiskAnalysisAsync(RiskAnalysis riskAnalysis)
        {
            if (riskAnalysis.TenantId == _tenantId)
            {
                _unitOfWork.RiskAnalysis.Update(riskAnalysis);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Güncelleme sonrası cache temizleme
            }
        }

        public async Task DeleteRiskAnalysisAsync(int id)
        {
            var riskAnalysis = await _unitOfWork.RiskAnalysis.GetByIdAsync(id);
            if (riskAnalysis != null && riskAnalysis.TenantId == _tenantId)
            {
                _unitOfWork.RiskAnalysis.Remove(riskAnalysis);
                await _unitOfWork.CompleteAsync();
                _cacheService.Remove(string.Format(_cacheKey, _tenantId)); // Silme sonrası cache temizleme
            }
        }
    }
}

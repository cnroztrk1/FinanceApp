using Data.UnitOfWork;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using System.Linq;

namespace FinanceApp.Business.Services
{
    public class RiskAnalysisService : IRiskAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _tenantId;

        public RiskAnalysisService(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IEnumerable<RiskAnalysis>> GetAllRiskAnalysesAsync()
        {
            return (await _unitOfWork.RiskAnalysis.GetAllAsync()).Where(ra => ra.TenantId == _tenantId);
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
        }

        public async Task UpdateRiskAnalysisAsync(RiskAnalysis riskAnalysis)
        {
            if (riskAnalysis.TenantId == _tenantId)
            {
                _unitOfWork.RiskAnalysis.Update(riskAnalysis);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteRiskAnalysisAsync(int id)
        {
            var riskAnalysis = await _unitOfWork.RiskAnalysis.GetByIdAsync(id);
            if (riskAnalysis != null && riskAnalysis.TenantId == _tenantId)
            {
                _unitOfWork.RiskAnalysis.Remove(riskAnalysis);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}

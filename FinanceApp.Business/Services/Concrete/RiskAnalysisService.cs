using Data.UnitOfWork;
using FinanceApp.Data;
using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public class RiskAnalysisService : IRiskAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RiskAnalysisService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RiskAnalysis>> GetAllRiskAnalysesAsync()
        {
            return await _unitOfWork.RiskAnalysis.GetAllAsync();
        }

        public async Task<RiskAnalysis> GetRiskAnalysisByIdAsync(int id)
        {
            return await _unitOfWork.RiskAnalysis.GetByIdAsync(id);
        }

        public async Task CreateRiskAnalysisAsync(RiskAnalysis riskAnalysis)
        {
            await _unitOfWork.RiskAnalysis.AddAsync(riskAnalysis);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateRiskAnalysisAsync(RiskAnalysis riskAnalysis)
        {
            _unitOfWork.RiskAnalysis.Update(riskAnalysis);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRiskAnalysisAsync(int id)
        {
            var riskAnalysis = await _unitOfWork.RiskAnalysis.GetByIdAsync(id);
            if (riskAnalysis != null)
            {
                _unitOfWork.RiskAnalysis.Remove(riskAnalysis);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}

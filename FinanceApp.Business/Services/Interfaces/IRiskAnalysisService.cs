using FinanceApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApp.Business.Services
{
    public interface IRiskAnalysisService
    {
        Task<IEnumerable<RiskAnalysis>> GetAllRiskAnalysesAsync();
        Task<RiskAnalysis> GetRiskAnalysisByIdAsync(int id);
        Task CreateRiskAnalysisAsync(RiskAnalysis riskAnalysis);
        Task UpdateRiskAnalysisAsync(RiskAnalysis riskAnalysis);
        Task DeleteRiskAnalysisAsync(int id);
    }
}

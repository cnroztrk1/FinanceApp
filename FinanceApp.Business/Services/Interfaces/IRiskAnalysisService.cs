using FinanceApp.Data.Entities;

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

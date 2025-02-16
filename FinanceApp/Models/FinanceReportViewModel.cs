using System.Collections.Generic;

namespace FinanceApp.Presentation.Models
{
    public class FinanceReportViewModel
    {
        public int TotalJobs { get; set; }
        public int TotalAgreements { get; set; }
        public decimal TotalRiskAmount { get; set; }
        public List<RiskAnalysisYearlyData> RiskAnalysisData { get; set; }
    }

    public class RiskAnalysisYearlyData
    {
        public int Year { get; set; }
        public decimal TotalRisk { get; set; }
    }
}

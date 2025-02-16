namespace FinanceApp.Presentation.Models
{
    public class RiskAnalysisViewModel
    {
        public int Id { get; set; }
        public decimal RiskAmount { get; set; }
        public string Comments { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string JobTitle { get; set; }
        public string BusinessPartnerName { get; set; }
        public string AgreementName { get; set; }
    }
}

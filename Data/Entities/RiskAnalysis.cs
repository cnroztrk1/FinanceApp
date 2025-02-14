using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinanceApp.Data.Entities
{
    public class RiskAnalysis
    {
        public int Id { get; set; }
        public decimal RiskAmount { get; set; }       
        public DateTime AnalysisDate { get; set; }      
        public string Comments { get; set; }           

        // Hangi anlaşma ve hangi iş için fk
        public int AgreementId { get; set; }
        [ValidateNever]
        public Agreement Agreement { get; set; }
        public int JobId { get; set; }
        [ValidateNever]
        public Jobs Job { get; set; }
    }
}

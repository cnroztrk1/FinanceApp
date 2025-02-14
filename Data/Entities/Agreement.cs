using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinanceApp.Data.Entities
{
    public class Agreement
    {
        public int Id { get; set; }
        public string Name { get; set; }             
        public string Description { get; set; }  
        public DateTime StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public bool IsActive { get; set; }

        // Anlaşmaya ait anahtar kelimeler , işler ve analizler fk
        [ValidateNever]
        public ICollection<AgreementKeys> Keywords { get; set; }
        [ValidateNever]
        public ICollection<Jobs> Jobs { get; set; }
        [ValidateNever]
        public ICollection<RiskAnalysis> RiskAnalyses { get; set; } 
    }
}

using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FinanceApp.Presentation.Models
{
    public class RiskAnalysisCreateViewModel
    {
        public RiskAnalysis RiskAnalysis { get; set; }
        // Sadece iş konularını seçmek için dropdown listesi
        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> Jobs { get; set; }
        // Seçilen işin ilişkili olduğu anlaşmayı (jobId -> agreementId) içeren JSON string
        [BindNever]
        [ValidateNever]
        public string JobAgreementsJson { get; set; }
    }
}

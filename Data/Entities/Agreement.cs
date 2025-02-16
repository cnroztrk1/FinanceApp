using Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace FinanceApp.Data.Entities
{
    public class Agreement : TenantBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        [ValidateNever]
        public ICollection<AgreementKeys> Keywords { get; set; }
        [ValidateNever]
        public ICollection<Jobs> Jobs { get; set; }
        [ValidateNever]
        public ICollection<RiskAnalysis> RiskAnalyses { get; set; }
    }
}

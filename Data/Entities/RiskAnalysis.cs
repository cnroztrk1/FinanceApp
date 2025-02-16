using Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace FinanceApp.Data.Entities
{
    public class RiskAnalysis : TenantBase //Risk Analizleri
    {
        public int Id { get; set; }
        public decimal RiskAmount { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string Comments { get; set; }
        public int AgreementId { get; set; }
        public int JobId { get; set; }

        [ValidateNever]
        public Agreement Agreement { get; set; }

        [ValidateNever]
        public Jobs Job { get; set; }
    }
}

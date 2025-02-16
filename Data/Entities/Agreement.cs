using Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace FinanceApp.Data.Entities
{
    public class Agreement : TenantBase //Anlaşmalar tablosu tenantbase den tenantId kalıtım alıyor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        //İlgili Collection nesneleri ihtiyaca bağlı veri çekmek için

        [ValidateNever]
        public ICollection<AgreementKeys> Keywords { get; set; } 
        [ValidateNever]
        public ICollection<Jobs> Jobs { get; set; }
        [ValidateNever]
        public ICollection<RiskAnalysis> RiskAnalyses { get; set; }
    }
}

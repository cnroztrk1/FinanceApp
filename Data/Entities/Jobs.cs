using System;
using System.ComponentModel.DataAnnotations;
using Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinanceApp.Data.Entities
{
    public class Jobs : TenantBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Status { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? AgreementId { get; set; }

        [ValidateNever]
        public Partners BusinessPartner { get; set; }

        [ValidateNever]
        public Agreement Agreement { get; set; }

        [ValidateNever]
        public RiskAnalysis RiskAnalysis { get; set; }
    }
}

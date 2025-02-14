using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinanceApp.Data.Entities
{
    public class Jobs
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "İş Ortağı seçimi gereklidir")]
        public int? BusinessPartnerId { get; set; }

        [Required(ErrorMessage = "Anlaşma seçimi gereklidir")]
        public int? AgreementId { get; set; }

        [ValidateNever]
        public Partners BusinessPartner { get; set; }

        [ValidateNever]

        public Agreement Agreement { get; set; }
        [ValidateNever]
        public RiskAnalysis RiskAnalysis { get; set; }
    }
}

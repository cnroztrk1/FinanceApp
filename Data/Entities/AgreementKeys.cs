using Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace FinanceApp.Data.Entities
{
    public class AgreementKeys : TenantBase
    {
        public int Id { get; set; }
        public int AgreementId { get; set; }
        public string Keyword { get; set; }
        public DateTime LastModifiedDate { get; set; }

        [ValidateNever]
        public Agreement Agreement { get; set; }
    }
}

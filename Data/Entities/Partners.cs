using Data.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace FinanceApp.Data.Entities
{
    public class Partners : TenantBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [ValidateNever]
        public ICollection<Jobs> Jobs { get; set; }
    }
}

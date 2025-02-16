using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Linq;

namespace Data.Common
{
    public class TenantBase
    {
        [ValidateNever]
        public int TenantId { get; set; } // Çok kiracılı yapı için
    }
}

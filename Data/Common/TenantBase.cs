using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common
{
    public class TenantBase
    {
        [ValidateNever]
        public int TenantId { get; set; } // Çok kiracılı yapı için
    }
}

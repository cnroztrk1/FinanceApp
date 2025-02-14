using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FinanceApp.Presentation.Models
{
    public class JobCreateViewModel
    {
        public Jobs Job { get; set; }
        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> BusinessPartners { get; set; }
        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> Agreements { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class CompanyAccountCreateViewModel
    {
        
        public int CompanyId { get; set; }

        [Required(ErrorMessage ="ErrorEmptyCarrency")]
        [Display(Name ="SelectCarrency")]
        public int? CurrencyId { get; set; }
        public SelectList Currencies { get; set; }

        public List<EmployeeAccountViewModel> EmployeeAccounts { get; set; }



    }
}

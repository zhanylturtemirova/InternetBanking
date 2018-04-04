using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class CreateBankViewModel
    {
        [Required(ErrorMessage = "ErrorBIKEmpty")]
        [Display(Name = "BIK")]
        public string BIK { get; set; }

        [Required(ErrorMessage = "ErrorNameEmpty")]
        [Display(Name = "Name")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "ErrorEmailEmpty")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}


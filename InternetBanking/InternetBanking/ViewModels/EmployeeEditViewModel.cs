using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class EmployeeEditViewModel
    {
        [Required(ErrorMessage = "ErrorEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "TwoFactorOn")]
        public bool TwoFactorOn { get; set; }

        public EmployeeInfo employee { get; set; }
    }
}

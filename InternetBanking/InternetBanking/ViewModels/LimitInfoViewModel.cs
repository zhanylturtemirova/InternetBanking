using InternetBanking.Models.SelectTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class LimitInfoViewModel
    {
        [Required (ErrorMessage = "RequiredErrorMessage")]
        [Display (Name = "LimitName")]
        public string LimitName { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "LimitAmount")]
        public string LimitAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class PaymentCodeInfoViewModel
    {

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "Code")]
        public string Code { get; set; }


        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "PaymentCodeName")]
        public string PaymentCodeName { get; set; }
    }
}

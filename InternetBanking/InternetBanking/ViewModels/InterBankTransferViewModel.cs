using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class InterBankTransferViewModel
    {
        [Display(Name = "Bank")]
        [Required(ErrorMessage = "BankErrorMessage")]
        public int? BankId { get; set; }
        public SelectList Banks { get; set; }

        [Display(Name = "PaymentCode")]
        [Required(ErrorMessage = "PaymentCodeErrorMessage")]
        public int? PaymentCodeId { get; set; }
        public SelectList PaymentCode { get; set; }

        [Display(Name = "ReciverName")]
        [Required(ErrorMessage = "ReciverNameErrorMessage")]
        public string ReciverName { get; set; }

        public InnerTransferViewModel Transfer { get; set; }


    }


}

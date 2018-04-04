using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class CurrencyConversionViewModel
    {
        public string Id { get; set; }
        [Display(Name = "AccountSenderName")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public int AccountSenderId { get; set; }

        [Display(Name = "AccountReceiveName")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public int AccountReceiverId { get; set; }
     
        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "AmountSend")]
        public string AmountSend { get; set; }
        public string AmountReceive { get; set; }
      
        public SelectList UserAccounts { get; set; }
        
    }
}

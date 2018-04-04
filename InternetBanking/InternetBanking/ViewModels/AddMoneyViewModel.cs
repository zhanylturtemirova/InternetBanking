using InternetBanking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class AddMoneyViewModel
    {
        public SelectList UserAccounts { get; set; }

        public Account Account { get; set; }
        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "AmountName")]
        public string Amount { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;

namespace InternetBanking.ViewModels
{
    public class AccountWithBalance
    {
        [Required]
        public Account Account { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}

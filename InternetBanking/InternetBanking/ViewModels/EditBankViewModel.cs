using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class EditBankViewModel
    {
        public CreateBankViewModel BankInfo { get; set; }
        public Bank Bank { get; set; }

        public EditBankViewModel()
        {
            BankInfo = new CreateBankViewModel();
        }
    }
}

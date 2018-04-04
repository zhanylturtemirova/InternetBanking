using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class UserIndexViewModel
    {
        public string UserName{ get; set; }
        public int UserId { get; set; }
        public List<AccountWithBalance> UserAccounts { get; set; }
        public List<ExchangeRate> NBKRRates { get; set; }
        public List<ExchangeRate> MarketRates { get; set; }

        public UserIndexViewModel()
        {

        }   
    }
}

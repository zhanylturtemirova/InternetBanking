using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class ExchangeRateForPartialViewViewModel
    {
       
        public string Name { get; set; }
        public decimal RateForSale { get; set; }
        public decimal RateForPurchaise { get; set; }
        
    }
}

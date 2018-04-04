using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class ExchangeRatesListViewModel
    {
        public IQueryable<ExchangeRate> exchangeRatesList { get; set; }
        public List<Currency> currencyList { get; set; }
    }
}

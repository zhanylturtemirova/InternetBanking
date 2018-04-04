using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class ExchangeRateViewModel
    {
        [Required(ErrorMessage = "ErrorEmptyRate")]
        [Display(Name = "RateForSale")]
        public string RateForSale { get; set; }
        [Required(ErrorMessage = "ErrorEmptyRate")]
        [Display(Name = "RateForPurchaise")]
        public string RateForPurchaise { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorEmptyCurrency")]
        [Display(Name = "SelectCurrency")]
        public int? CurrencyId { get; set; }
        public IEnumerable<Currency> CurrencyList { get; set; }

        //[Required(ErrorMessage = "ErrorEmptyType")]
        //[Display(Name = "SelectType")]
        //public int TypeId { get; set; }
        //public IEnumerable<ExchangeRateType> TypeList { get; set; }
    }
}

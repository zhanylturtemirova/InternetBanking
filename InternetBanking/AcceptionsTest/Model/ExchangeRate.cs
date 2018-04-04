using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AcceptionsTest.Model
{
    public class ExchangeRate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorEmptyRate")]
        [Display(Name = "КурсПродажи")]
        public decimal RateForSale { get; set; }
        [Required(ErrorMessage = "ErrorEmptyRate")]
        [Display(Name = "КурсПокупки")]
        public decimal RateForPurchaise{ get; set; }
     

        public DateTime RateDate { get; set; }
        public int? CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public int ExchangeRateTypeId { get; set; }
        public ExchangeRateType ExchangeRateType { get; set; }
    }
}

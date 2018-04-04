using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace AcceptionsTest.Model
{
    public class ExchangeRateType
    {
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        public static ExchangeRateType Create(ExchangeRateTypesEnum enumItem)
        {
            return new ExchangeRateType{Name = enumItem.ToString()};
        }

        public bool IsEqual(ExchangeRateTypesEnum enumItem)
        {
            return this.Name == enumItem.ToString();
        }
    }
    public enum ExchangeRateTypesEnum
    {
        NBKR = 1,
        Market = 2

    }
}

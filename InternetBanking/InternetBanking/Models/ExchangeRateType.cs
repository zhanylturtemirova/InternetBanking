using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Models
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
}

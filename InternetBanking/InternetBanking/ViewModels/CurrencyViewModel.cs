using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class CurrencyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorNameEmpty")]
        [Display(Name = "NameCurrency")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ErrorCodeEmpty")]
        [Display(Name = "CodeCurrency")]
        public string Code { get; set; }

        public bool IsNativeCurrency { get; set; }
    }
}

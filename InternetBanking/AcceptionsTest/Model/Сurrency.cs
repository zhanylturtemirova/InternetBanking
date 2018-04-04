using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AcceptionsTest.Model
{
    public class Currency
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorNameEmpty")]
        [Display(Name = "NameCurrency")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ErrorCodeEmpty")]
        [Display(Name = "CodeCurrency")]
        public string Code { get; set; }
        public bool IsNativeCurrency { get; set; }


       

        public Currency()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class BlackListPasswordViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorNameEmpty")]
        [Display(Name = "BlackListedPassword")]
        public string Name { get; set; }
    }
}

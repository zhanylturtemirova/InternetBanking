using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="ErrorEmailEmpty")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ErrorPasswordEmpty")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }
}

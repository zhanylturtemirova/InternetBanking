
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;

namespace InternetBanking.ViewModels
{
    public class TwoFactorAuthViewModel
    {

        public string UserId { get; set; }
        public User User { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Code")]
        public string Code { get; set; }


        [Compare("Code")]
        [DataType(DataType.Password)]
        [Display(Name = "CodeConfirm")]
        public string CodeConfirm { get; set; }

        public DateTime SendCodeTime { get; set; }
    }
}

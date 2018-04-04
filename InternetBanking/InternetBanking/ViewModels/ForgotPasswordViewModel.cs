using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.ViewModels
{
    public class ForgotPasswordViewModel
    {

        public string Id { get; set; }
       

        [Required(ErrorMessage = "ErrorEmptyNewPassword")]
        [StringLength(100, ErrorMessage = "PasswordMinLength", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        [Remote(action: "CheckPasswordBlackList", controller: "Manage", ErrorMessage = "Данный пароль ненадежен")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessage = "ErrorCompare")]
        public string ConfirmPassword { get; set; }
    }
}

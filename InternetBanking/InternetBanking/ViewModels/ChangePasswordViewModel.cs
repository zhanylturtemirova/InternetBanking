using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.ViewModels
{
    public class ChangePasswordViewModel
    {

            public string Id { get; set; }
            public string Email { get; set; }

            [Required (ErrorMessage = "ErrorEmptyCurrentPassword")]
            [DataType(DataType.Password)]
            [Display(Name = "CurrentPassword")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "ErrorEmptyNewPassword")]
            [StringLength(100, ErrorMessage = "PasswordMinLength", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "NewPassword")]
            [Remote(action: "CheckPasswordBlackList", controller: "Manage", ErrorMessage = "Данный пароль ненадежен")]
            public string NewPassword { get; set; }

          
            [Display(Name = "ConfirmNewPassword")]
            [Compare("NewPassword", ErrorMessage = "ErrorCompare")]
            public string ConfirmPassword { get; set; }
        
    }
}

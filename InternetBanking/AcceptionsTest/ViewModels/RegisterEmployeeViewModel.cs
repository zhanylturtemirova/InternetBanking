using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class RegisterEmployeeViewModel
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "ErrorEmailEmpty")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "ErrorFirstNameEmpty")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "ErrorSecondNameEmpty")]
        [Display(Name = "SecondName")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "ErrorMiddleNameEmpty")]
        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "ErrorPositionEmpty")]
        [Display(Name = "Position")]
        public string Position { get; set; }


        [Display(Name = "TwoFactorOn")]
        public bool TwoFactorOn { get; set; }
        public int CompanyId { get; set; }

    }
}

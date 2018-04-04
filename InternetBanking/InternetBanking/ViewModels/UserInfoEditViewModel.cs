using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class UserInfoEditViewModel
    {
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


        [Required(ErrorMessage = "ErrorINNEmpty")]
        [Display(Name = "INN")]
        //[RegularExpression(@"^[A-Z\s]{2}\.-]+\d{7}$", ErrorMessage = "неверный формат ИНН")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "неверный формат ИНН")]
        public string Inn { get; set; }

        [Required(ErrorMessage = "ErrorGenderEmpty")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "ErrorBirthDayEmpty")]
        [Display(Name = "BirthDay")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Citizenship")]
        public int? CountryId { get; set; }
        public SelectList Countries { set; get; }

        [Display(Name = "TwoFactorOn")]
        public bool TwoFactorOn { get; set; }
    }
}

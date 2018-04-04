using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class ContactInfoViewModel
    {
        [Required(ErrorMessage = "ErrorMobilePhone")]
        [Display(Name = "MobilePhone")]
        public string MobilePhone { get; set; }
        [Display(Name = "CityPhone")]
        public string CityPhone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "FullName")]
        public string FullName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}

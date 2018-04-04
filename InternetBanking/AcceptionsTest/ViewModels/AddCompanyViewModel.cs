using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class AddCompanyViewModel
    {

        public FactAddressViewModel FactAddress { get; set; }
        public LegalAddressViewModel LegalAddress { get; set; }
        public ContactInfoViewModel ContactInfo { get; set; }
        public AddCompanyViewModel()
        {
            FactAddress = new FactAddressViewModel();
            LegalAddress = new LegalAddressViewModel();
            ContactInfo = new ContactInfoViewModel();
        }

 
        [Required(ErrorMessage = "ErrorNameCompanyEmpty")]
        [Display(Name = "NameCompany")]
        public string NameCompany { get; set; }

        [Required(ErrorMessage = "ErrorINNEmpty")]
        [Display(Name = "IIN")]
        public string InnCompany { get; set; }

        [Required(ErrorMessage = "ErrorOKPOEmpty")]
        [Display(Name = "OKPO")]
        public string OkpoCompany { get; set; }

        [Required(ErrorMessage = "ErrorRegistrationNumberSocialFundEmpty")]
        [Display(Name = "RegistrationNumberSocialFund")]
        public string RegistrationNumberSocialFund { get; set; }

        [Required(ErrorMessage = "ErrorRegistrationAuthorityEmpty")]
        [Display(Name = "RegistrationAuthority")]
        public string RegistrationAuthority { get; set; }

        [Required(ErrorMessage = "ErrorIssuedByEmpty")]
        [Display(Name = "IssuedBy")]
        public string IssuedBy { get; set; }

        [Required(ErrorMessage = "ErrorDateOfRegistrationMinistryJusticeEmpty")]
        [Display(Name = "DateOfRegistrationMinistryJustice")]
        public string DateOfRegistrationMinistryJustice { get; set; }

        [Required(ErrorMessage = "ErrorDateOfInitialRegistrationEmpty")]
        [Display(Name = "DateOfInitialRegistration")]
        public string DateOfInitialRegistration { get; set; }

        [Display(Name = "NumberOfEmployees")]
        [Range(0, int.MaxValue, ErrorMessage = "NumberOfEmployeesRangeErrorMessage")]
        public int NumberOfEmployees { get; set; }
    }
}

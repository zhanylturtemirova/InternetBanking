using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;

namespace InternetBanking.ViewModels
{
    public class RegistrationDataViewModel
    {
        public int Id { get; set; }

        [Display(Name = "RegistrationAuthority")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public string RegistrationAuthority { get; set; }

        [Display(Name = "DateOfRegistrationMinistryJustice")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public DateTime DateOfRegistrationMinistryJustice { get; set; }

        [Display(Name = "IssuedBy")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public string IssuedBy { get; set; }

        [Display(Name = "DateOfInitialRegistration")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public DateTime DateOfInitialRegistration { get; set; }


        [Display(Name = "TaxInspectionId")]
        [Required(ErrorMessage = "ErrorTaxInspectionEmpty")]
        public int? TaxInspectionId { get; set; }
        public TaxInspection TaxInspection { get; set; }

        public RegistrationDataViewModel()
        {
        }

        public RegistrationDataViewModel(RegistrationData registrationData)
        {
            Id = registrationData.Id;
            RegistrationAuthority = registrationData.RegistrationAuthority;
            DateOfRegistrationMinistryJustice = registrationData.DateOfRegistrationMinistryJustice;
            IssuedBy = registrationData.IssuedBy;
            DateOfInitialRegistration = registrationData.DateOfInitialRegistration;
            TaxInspectionId = registrationData.TaxInspectionId;
            TaxInspection = registrationData.TaxInspection;
        }
    }
}

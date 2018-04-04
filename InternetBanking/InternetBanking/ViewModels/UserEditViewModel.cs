using InternetBanking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class UserEditViewModel
    {
        public UserInfo User { get; set; }
        public FactAddressViewModel FactAddress { get; set; }
        public PlaceOfBirthViewModel PlaceOfBirth { get; set; }
        public LegalAddressViewModel LegalAddress { get; set; }
        public UserInfoEditViewModel UserInfo { get; set; }
        public PassportInfoEditViewModel PassportInfo { get; set; }
        public ContactInfoViewModel ContactInfo { get; set; }

        public UserEditViewModel()
        {
            FactAddress = new FactAddressViewModel();
            PlaceOfBirth = new PlaceOfBirthViewModel();
            LegalAddress = new LegalAddressViewModel();
            UserInfo = new UserInfoEditViewModel();
            PassportInfo = new PassportInfoEditViewModel();
            ContactInfo = new ContactInfoViewModel();
        }

    }
}

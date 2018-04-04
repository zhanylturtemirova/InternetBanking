using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcceptionsTest.ViewModels
{
    public class RegisterPersonViewModel
    {
        public FactAddressViewModel FactAddress { get; set; }
        public PlaceOfBirthViewModel PlaceOfBirth { get; set; }
        public LegalAddressViewModel LegalAddress { get; set; }
        public UserInfoViewModel UserInfo { get; set; }
        public PassportInfoViewModel PassportInfo { get; set; }
        public ContactInfoViewModel ContactInfo { get; set; }

        public RegisterPersonViewModel()
        {
            FactAddress = new FactAddressViewModel();
            PlaceOfBirth = new PlaceOfBirthViewModel();
            ContactInfo = new ContactInfoViewModel();
            LegalAddress = new LegalAddressViewModel();
            UserInfo = new UserInfoViewModel();
            PassportInfo = new PassportInfoViewModel();
        }
    }
}

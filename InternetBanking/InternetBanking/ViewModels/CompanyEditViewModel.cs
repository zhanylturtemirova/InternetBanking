using InternetBanking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class CompanyEditViewModel
    {
        public FactAddressViewModel FactAddress { get; set; }
        public LegalAddressViewModel LegalAddress { get; set; }
        public ContactInfoViewModel ContactInfo { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public CompanyEditViewModel()
        {
            FactAddress = new FactAddressViewModel();
            LegalAddress = new LegalAddressViewModel();
            ContactInfo = new ContactInfoViewModel();

        }

        
        public SelectList Countries { set; get; }

       
        public SelectList LegalForms { set; get; }

  
        public SelectList PropertyTypes { set; get; }

     
        public SelectList Residencies { set; get; }

      
        public SelectList TaxInspections { set; get; }

        public CompanyViewModel Company { get; set; }
        public RegistrationDataViewModel RegistrationData { get; set; }

        public IFormFile Logo { get; set; }

        public string LogoPath { get; set; }
    }
}

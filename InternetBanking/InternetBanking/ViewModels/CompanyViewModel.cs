using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace InternetBanking.ViewModels
{
    public class CompanyViewModel : IPageble
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage= "RequiredErrorMessage")]
        [Display(Name="NameCompany")]
        public string NameCompany { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "LegalFormId")]
        public int? LegalFormId { get; set; }
        public LegalForm LegalForm { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "PropertyTypeId")]
        public int? PropertyTypeId { get; set; }
        public  PropertyType PropertyType { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "INN")]
        public string INN { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "CodeOKPO")]
        public string CodeOKPO { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "RegistrationNumberSocialFund")]
        public string RegistrationNumberSocialFund { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "ResidencyId")]
        public int? ResidencyId { get; set; }
        public Residency Residency { get; set; }

        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "CountryId")]
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "RegistrationDataId")]
        public int RegistrationDataId { get; set; }

       
        [Display(Name = "ContactInfoId")]
        public int? ContactInfoId { get; set; }


        [Range(0, int.MaxValue, ErrorMessage= "NumberOfEmployeesErrorMessage")]
        [Display(Name= "NumberOfEmployees")]
        public int NumberOfEmployees { get; set; }

        [Display(Name="Logo")]
        public string Logo { get; set; }

       
        public CompanyViewModel()
        {
        }

        public CompanyViewModel(Company company)
        {
            NameCompany = company.NameCompany;
            CodeOKPO = company.CodeOKPO;
            ContactInfoId = company.ContactInfoId;
            CountryId = company.CountryId;
            Country = company.Country;
            Id = company.Id;
            INN = company.INN;
            LegalFormId = company.LegalFormId;
            LegalForm = company.LegalForm;
            NumberOfEmployees = company.NumberOfEmployees;
            PropertyTypeId = company.PropertyTypeId;
            PropertyType = company.PropertyType;
            RegistrationNumberSocialFund = company.RegistrationNumberSocialFund;
            RegistrationDataId = company.RegistrationDataId;
            ResidencyId = company.ResidencyId;
            Residency = company.Residency;
            Logo = company.Logo;
        }


        public int GetPageSize()
        {
            return 3;
        }
    
    }
}

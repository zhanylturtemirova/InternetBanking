using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Services
{
    public interface ICompanyService
    {
        Company CreateCompany(AddCompanyViewModel model, RegistrationData registrData, ContactInfo contactInfo);
        RegistrationData CreateRegistrationDate(AddCompanyViewModel model);
        IQueryable<Company> GetCompanies();
        Company FindCompanyById(int id);
        void UpdateCompany(CompanyEditViewModel model);
        bool IsExist(int id);
        RegistrationData FindRegistrationDataById(int id);
        void CreateAddress(Company company, LegalAddressViewModel model);
        void CreateAddress(Company company, FactAddressViewModel model);
        Task<ContactInfo> CreateContactInfo(ContactInfoViewModel model);
        string GetCompanyLogo(int companyId);
        Address FindFactAddressByCompanyId(int companyId);
        Address FindLegalAddressByCompanyId(int companyId);
        void AddRangeEmployeeAccounts(List<EmployeeAccountViewModel> employeeAccountViewModels);
        
    }
}

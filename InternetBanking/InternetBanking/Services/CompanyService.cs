using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Hosting;

namespace InternetBanking.Services
{
    public class CompanyService : ICompanyService
    {
        private ApplicationContext context;
        private IHostingEnvironment environment;
        private FileUploadService fileUploadService;

        public CompanyService( ApplicationContext context, IHostingEnvironment environment,
            FileUploadService fileUploadService)
        {
            this.environment = environment;
            this.fileUploadService = fileUploadService;
            this.context = context;
        }

        public RegistrationData CreateRegistrationDate(AddCompanyViewModel model)
        {
            RegistrationData registrData = new RegistrationData
            {
                RegistrationAuthority = model.RegistrationAuthority,
                DateOfRegistrationMinistryJustice = DateTime.Parse(model.DateOfRegistrationMinistryJustice),
                IssuedBy = model.IssuedBy,
                DateOfInitialRegistration = DateTime.Parse(model.DateOfInitialRegistration),
                TaxInspectionId = model.TaxInspectionId
            };
            context.RegistrationDatas.Add(registrData);
            context.SaveChanges();
            return registrData;
        }

        public async Task<ContactInfo> CreateContactInfo(ContactInfoViewModel model)
        {
            ContactInfo contactInfo = new ContactInfo { MobilePhone = model.MobilePhone, CityPhone = model.CityPhone, Email = model.Email };
            await context.ContactInfos.AddAsync(contactInfo);
            context.SaveChanges();
            return contactInfo;
        }

        public Company CreateCompany(AddCompanyViewModel model, RegistrationData registrData, ContactInfo contactInfo)
        {
            string logoPath = String.Empty;
            if (model.Logo != null)
            {
                var path = Path.Combine(
                    environment.WebRootPath,
                    $"images\\{model.NameCompany}\\Logo");
                fileUploadService.Upload(path, model.Logo.FileName, model.Logo);
                logoPath = $"images/{model.NameCompany}/Logo/{model.Logo.FileName}";
            }
          

            Company company = new Company
            {
                NameCompany = model.NameCompany,
                LegalFormId = model.LegalFormId,
                PropertyTypeId = model.PropertyTypeId,
                INN = model.InnCompany,
                CodeOKPO = model.OkpoCompany,
                RegistrationNumberSocialFund = model.RegistrationNumberSocialFund,
                ResidencyId = model.ResidencyId,
                CountryId = model.CountryId,
                NumberOfEmployees = model.NumberOfEmployees,
                RegistrationDataId = registrData.Id,
                ContactInfoId = contactInfo.Id,
                Logo = logoPath
            };

            context.Companies.Add(company);
            context.SaveChanges();
            return company;
        }

        public IQueryable<Company> GetCompanies()
        {
            IQueryable<Company> companies = context.Companies.Include(c => c.RegistrationData).ThenInclude(r => r.TaxInspection).Include(c => c.Country).Include(c => c.PropertyType).Include(c => c.LegalForm).Include(c => c.ContactInfo).Include(c => c.EmployeeInfos).Include(c => c.Accounts).Include(c =>c.Residency);

            return companies;
        }

        public Company FindCompanyById(int id)
        {
            Company company = GetCompanies().FirstOrDefault(c => c.Id == id);
            return company;
        }

        public RegistrationData FindRegistrationDataById(int id)
        {
            RegistrationData registrationData = context.RegistrationDatas.FirstOrDefault(r => r.Id == id);
            return registrationData;
        }

        public ContactInfo FindContactInfoById(int? id)
        {
            ContactInfo contactInfo = context.ContactInfos.FirstOrDefault(c => c.Id == id);
            return contactInfo;
        }

        public async void UpdateCompany(CompanyEditViewModel model)
        {
            
            Company company = FindCompanyById(model.Company.Id);
            company.NameCompany = model.Company.NameCompany;
            company.ResidencyId = model.Company.ResidencyId;
            company.RegistrationNumberSocialFund = model.Company.RegistrationNumberSocialFund;
            company.INN = model.Company.INN;
            company.CodeOKPO = model.Company.CodeOKPO;
            company.CountryId = model.Company.CountryId;
            company.NumberOfEmployees = model.Company.NumberOfEmployees;
            company.LegalFormId = model.Company.LegalFormId;
            company.PropertyTypeId = model.Company.PropertyTypeId;
            if (model.Logo != null)
            {
                var path = Path.Combine(
                    environment.WebRootPath,
                    $"images\\{model.Company.NameCompany}\\Logo");
                fileUploadService.Upload(path, model.Logo.FileName, model.Logo);

                company.Logo = $"images/{model.Company.NameCompany}/Logo/{model.Logo.FileName}";
            }

            ContactInfo contactInfo = FindContactInfoById(company.ContactInfoId);
            if (contactInfo != null)
            {
                contactInfo.MobilePhone = model.ContactInfo.MobilePhone;
                contactInfo.CityPhone = model.ContactInfo.CityPhone;
                contactInfo.Email = model.ContactInfo.Email;
                context.ContactInfos.Update(contactInfo);
            }
            else
            {
                await CreateContactInfo(model.ContactInfo);
            }
            Address factAddress =  FindFactAddressByCompanyId(company.Id);
            if (factAddress != null)
            {
                factAddress.City = model.FactAddress.City;
                factAddress.CountryId = model.FactAddress.CountryId;
                factAddress.HouseAddress = model.FactAddress.HouseAddress;
                factAddress.PostCode = model.FactAddress.PostCode;
                factAddress.Street = model.FactAddress.Street;

                 context.Addresses.Update(factAddress);
            }
            else
            {
                CreateAddress(company, model.FactAddress);
            }
            Address legalAddress =  FindLegalAddressByCompanyId(company.Id);
               
            if (legalAddress != null)
            {
                legalAddress.City = model.LegalAddress.City;
                legalAddress.CountryId = model.LegalAddress.CountryId;
                legalAddress.HouseAddress = model.LegalAddress.HouseAddress;
                legalAddress.PostCode = model.LegalAddress.PostCode;
                legalAddress.Street = model.LegalAddress.Street;

                context.Addresses.Update(legalAddress);
            }
            else
            {
                CreateAddress(company, model.LegalAddress);
            }
            RegistrationData registrationData =
                context.RegistrationDatas.FirstOrDefault(r => r.Id == model.RegistrationData.Id);


            registrationData.DateOfInitialRegistration = model.RegistrationData.DateOfInitialRegistration;
            registrationData.DateOfRegistrationMinistryJustice = model.RegistrationData.DateOfRegistrationMinistryJustice;
            registrationData.IssuedBy = model.RegistrationData.IssuedBy;
            registrationData.RegistrationAuthority = model.RegistrationData.RegistrationAuthority;
            registrationData.TaxInspectionId = model.RegistrationData.TaxInspectionId;


            context.RegistrationDatas.Update(registrationData);
            context.Companies.Update(company);
           
            context.SaveChanges();
        }

        public bool IsExist(int id)
        {   
            return FindCompanyById(id) != null;
        }


        public async void CreateAddress(Company company, FactAddressViewModel model)
        {
            Address factAddress = new Address { CountryId = model.CountryId, City = model.City, PostCode = model.PostCode, Street = model.Street, HouseAddress = model.HouseAddress, CompanyId = company.Id, AddressType = AddressType.Create(AddressTypesEnum.FactAddress) };
            await context.Addresses.AddAsync(factAddress);
            context.SaveChanges();
        }

        public async void CreateAddress(Company company, LegalAddressViewModel model)
        {
            Address legalAddress = new Address { CountryId = model.CountryId, City = model.City, PostCode = model.PostCode, Street = model.Street, HouseAddress = model.HouseAddress, CompanyId = company.Id, AddressType = AddressType.Create(AddressTypesEnum.LegalAddress) };
            await context.Addresses.AddAsync(legalAddress);
            context.SaveChanges();

        }

        public string GetCompanyLogo(int companyId)
        {
            string logo = context.Companies.FirstOrDefault(c => c.Id == companyId).Logo;
             
            return logo;
        }

        public  Address FindFactAddressByCompanyId(int companyId)
        {
             Address factAddress =  context.Addresses.FirstOrDefault(a=>a.CompanyId == companyId && a.AddressType.TypeName == AddressTypesEnum.FactAddress.ToString());
            return factAddress;
        }

        public Address FindLegalAddressByCompanyId(int companyId)
        {
            Address legalAddress = 
                context.Addresses.FirstOrDefault(
                    a => a.CompanyId == companyId && a.AddressType.TypeName == AddressTypesEnum.LegalAddress.ToString());

            return legalAddress;
        }

        public void AddRangeEmployeeAccounts(List<EmployeeAccountViewModel> employeeAccountViewModels)
        {

            var employeeAccounts = employeeAccountViewModels.Select(e => new EmployeeAccount
            {
                EmployeeId = e.EmployeeId,
                Account = e.Account,
                RightOfCreate = e.RightOfCreate,
                RightOfConfirmation = e.RightOfConfirmation,
                LimitId = e.LimitId
            });
            foreach (var employeeAccount in employeeAccounts)
            {
                EmployeeInfo employee = context.EmployeeInfos.FirstOrDefault(e => e.Id == employeeAccount.EmployeeId);
                if (employee.Chief)
                {
                    employeeAccount.RightOfConfirmation = true;
                    employeeAccount.RightOfCreate = true;
                }
            }
            context.EmployeeAccounts.AddRange(employeeAccounts);
            context.SaveChanges();
        }

        
    } 
}

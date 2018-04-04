using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InternetBanking.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InternetBanking.Controllers
{
    public class CompanyController : Controller
    {
        private ApplicationContext context;
        private readonly ISelectListService selectListService;
        private readonly ICompanyService companyService;
        private readonly IHomePagingService pagingService;
        private readonly IEmployeeService employeeService;
        private readonly IAccountService accountService;
        private readonly IValidationService validationService;

        public CompanyController(ApplicationContext context, ISelectListService selectListService, ICompanyService companyService, IHomePagingService pagingService, IEmployeeService employeeService, IAccountService accountService, IValidationService validationService)
        {
            this.context = context;
            this.selectListService = selectListService;
            this.companyService = companyService;
            this.pagingService = pagingService;
            this.employeeService = employeeService;
            this.accountService = accountService;
            this.validationService = validationService;
        }

        [HttpGet]

        public IActionResult AddCompany()
        {

            AddCompanyViewModel company = new AddCompanyViewModel
            {
                FactAddress = new FactAddressViewModel{ Countries = selectListService.GetCountries()},
                LegalAddress = new LegalAddressViewModel{Countries = selectListService.GetCountries()},
                ContactInfo = new ContactInfoViewModel()
            };
            company = selectListService.GetCompaniesSelectList(company);
            return View(company);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany(AddCompanyViewModel model)
        {
             validationService.ValidateAddCompanyViewModel(model, ModelState);

            if (ModelState.IsValid)
            {
                ContactInfo contactInfo = await companyService.CreateContactInfo(model.ContactInfo);
                RegistrationData registrData = companyService.CreateRegistrationDate(model);
                Company company = companyService.CreateCompany(model, registrData, contactInfo);
                companyService.CreateAddress(company,model.FactAddress);
                companyService.CreateAddress(company,model.LegalAddress);
                return RedirectToAction("RegisterEmployeeCompany", "User", new { companyId = company.Id });
            }
            else
            {
                model.FactAddress.Countries = selectListService.GetCountries();
                model.LegalAddress.Countries = selectListService.GetCountries();
                model = selectListService.GetCompaniesSelectList(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Company> companies = companyService.GetCompanies().OrderBy(c => c.NameCompany); 
            PagedObject<Company> pagedObject = await pagingService.DoPage<Company>(companies, page);

            PagingViewModel<Company> CompaniesPagingViewModel = new PagingViewModel<Company>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            return View(CompaniesPagingViewModel);
        }

        [HttpGet]
        public IActionResult CompanyInfo(int id)
        {
            Company company = companyService.GetCompanies().FirstOrDefault(c => c.Id == id);
            RegistrationData registrationData = companyService.FindRegistrationDataById(company.RegistrationDataId);
            CompanyViewModel companyViewModel = new CompanyViewModel(company);
            RegistrationDataViewModel registrationDataViewModel = new RegistrationDataViewModel(registrationData);
            FactAddressViewModel factAddressViewModel = new FactAddressViewModel();
            ContactInfoViewModel contactInfoViewModel = new ContactInfoViewModel
            {
                CityPhone = company.ContactInfo.CityPhone,
                MobilePhone = company.ContactInfo.MobilePhone,
                Email = company.ContactInfo.Email
                
            };
            Address factAddress = companyService.FindFactAddressByCompanyId(company.Id);

            if (factAddress != null)
            {

                factAddressViewModel.CountryId = factAddress.CountryId;
                factAddressViewModel.City = factAddress.City;
                factAddressViewModel.Street = factAddress.Street;
                factAddressViewModel.HouseAddress = factAddress.HouseAddress;
                factAddressViewModel.PostCode = factAddress.PostCode;
            }

            LegalAddressViewModel legalAddressViewModel = new LegalAddressViewModel();
            Address legalAddress = companyService.FindLegalAddressByCompanyId(company.Id);
            if (factAddress != null)
            {

                legalAddressViewModel.CountryId = legalAddress.CountryId;
                legalAddressViewModel.City = legalAddress.City;
                legalAddressViewModel.Street = legalAddress.Street;
                legalAddressViewModel.HouseAddress = legalAddress.HouseAddress;
                legalAddressViewModel.PostCode = legalAddress.PostCode;
            }

            CompanyEditViewModel model = new CompanyEditViewModel
            {
                FactAddress = factAddressViewModel,
                LegalAddress = legalAddressViewModel,
                Company = companyViewModel,
                RegistrationData = registrationDataViewModel,
                ContactInfo = contactInfoViewModel,
                EmployeeInfos = company.EmployeeInfos,
                Accounts = company.Accounts,
                LogoPath = company.Logo

               
               
            };
            model.Countries = selectListService.GetCountries();
            Country fcountry = context.Countries.FirstOrDefault(f => f.Id == factAddress.CountryId);
            Country lcountry = context.Countries.FirstOrDefault(f => f.Id == legalAddress.CountryId);
            model.Countries = selectListService.GetCountries();
            if (fcountry != null)
            {
                model.FactAddress.CountryName = fcountry.CountryName;
            }
            if (lcountry != null)
            {
                model.LegalAddress.CountryName = lcountry.CountryName;
            }
            
            model.LegalForms = selectListService.GetLegalForms();
            model.PropertyTypes = selectListService.GetPropertyTypes();
            model.Residencies = selectListService.GetResidencies();
            model.TaxInspections = selectListService.GetTaxInspections();
            ViewBag.CountriesForAddress = GetCountriesForAddress();
            //model.Logo = 
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            Company company = companyService.GetCompanies().FirstOrDefault(c => c.Id == id);
            RegistrationData registrationData = companyService.FindRegistrationDataById(company.RegistrationDataId);
            CompanyViewModel companyViewModel = new CompanyViewModel(company);
            RegistrationDataViewModel registrationDataViewModel = new RegistrationDataViewModel(registrationData);
            FactAddressViewModel factAddressViewModel = new FactAddressViewModel();
            ContactInfoViewModel contactInfoViewModel = new ContactInfoViewModel
            {
                CityPhone = company.ContactInfo.CityPhone,
                MobilePhone = company.ContactInfo.MobilePhone,
                Email = company.ContactInfo.Email
            };
            Address factAddress = companyService.FindFactAddressByCompanyId(company.Id);
            if (factAddress != null)
            {

                factAddressViewModel.CountryId = factAddress.CountryId;
                factAddressViewModel.City = factAddress.City;
                factAddressViewModel.Street = factAddress.Street;
                factAddressViewModel.HouseAddress = factAddress.HouseAddress;
                factAddressViewModel.PostCode = factAddress.PostCode;
            }

            LegalAddressViewModel legalAddressViewModel = new LegalAddressViewModel();
            Address legalAddress = companyService.FindLegalAddressByCompanyId(company.Id);
            if (factAddress != null)
            {

                legalAddressViewModel.CountryId = legalAddress.CountryId;
                legalAddressViewModel.City = legalAddress.City;
                legalAddressViewModel.Street = legalAddress.Street;
                legalAddressViewModel.HouseAddress = legalAddress.HouseAddress;
                legalAddressViewModel.PostCode = legalAddress.PostCode;
            }

            CompanyEditViewModel model = new CompanyEditViewModel
            {
                FactAddress = factAddressViewModel,
                LegalAddress = legalAddressViewModel,
                Company = companyViewModel,
                RegistrationData = registrationDataViewModel,
                ContactInfo = contactInfoViewModel
            };
            model.Countries = selectListService.GetCountries();
            model.LegalForms = selectListService.GetLegalForms();
            model.PropertyTypes = selectListService.GetPropertyTypes();
            model.Residencies = selectListService.GetResidencies();
            model.TaxInspections = selectListService.GetTaxInspections();
            ViewBag.CountriesForAddress = GetCountriesForAddress();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CompanyEditViewModel model)
        {

            validationService.ValidateCompanyEditViewModel(model, ModelState);

            if (ModelState.IsValid)
            {
                companyService.UpdateCompany(model);

                return RedirectToAction("Index", "Company");
            }
            model.Countries = selectListService.GetCountries();
            model.Company.Logo = companyService.GetCompanyLogo(model.Company.Id);
            model.LegalForms = selectListService.GetLegalForms();
            model.PropertyTypes = selectListService.GetPropertyTypes();
            model.Residencies = selectListService.GetResidencies();
            model.TaxInspections = selectListService.GetTaxInspections();
            ViewBag.CountriesForAddress = GetCountriesForAddress();

            return View(model);
        }


        private SelectList GetCountriesForAddress()
        {
            var countries = context.Countries;
            List<SelectListItem> selectList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Выберите страну", Value = "" }
            };
            foreach (var country in countries)
            {
                selectList.Add(new SelectListItem { Text = country.CountryName, Value = country.Id.ToString(), Selected = false });
            }
            return new SelectList(selectList, "Value", "Text");
        }




    }
}
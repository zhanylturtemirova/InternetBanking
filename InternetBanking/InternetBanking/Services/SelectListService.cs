using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class SelectListService : ISelectListService
    {
        private readonly ApplicationContext context;

        public SelectListService(ApplicationContext context)
        {
            this.context = context;
        }

        public SelectList GetCountries()
        {


            return new SelectList(context.Countries.ToList(), "Id", "CountryName");
        }





        public SelectList GetLegalForms()
        {
            return new SelectList(context.LegalForms.ToList(), "Id", "LegalFormName");
        }

        public SelectList GetPropertyTypes()
        {
            return new SelectList(context.PropertyTypes.ToList(), "Id", "PropertyTypeName");
        }

        public SelectList GetResidencies()
        {
            return new SelectList(context.Residencies.ToList(), "Id", "ResidencyName");
        }

        public SelectList GetTaxInspections()
        {
            return new SelectList(context.TaxInspections.ToList(), "Id", "TaxInspectionName");
        }

        public SelectList GetCurrencies()
        {
            return new SelectList(context.Currencies.ToList(), "Id", "Name");
        }

        public SelectList GetLimits()
        {
            return new SelectList(context.Limits.ToList(), "Id", "LimitName");
        }

        public SelectList GetUserAccounts(int userInfoId)
        {
            return new SelectList(context.Accounts.Where(a => a.UserInfoId == userInfoId), "Id", "Number");
        }

        public SelectList GetCompanyAccounts(int companyId)
        {
            return new SelectList(context.Accounts.Where(a => a.CompanyId == companyId), "Id", "Number");
        }

        public AddCompanyViewModel GetCompaniesSelectList(AddCompanyViewModel company)
        {
            company.Countries = GetCountries();
            company.LegalForms = GetLegalForms();
            company.PropertyTypes = GetPropertyTypes();
            company.Residencies = GetResidencies();
            company.TaxInspections = GetTaxInspections();
            return company;
        }

        public SelectList GetTypeOfDocuments()
        {
            return new SelectList(context.TypeOfDocuments.ToList(), "Id", "Name");
        }

        public SelectList GetEmployeeAccounts(int employeeInfoId)
        {
            List<Account> accounts;
            EmployeeInfo employee = context.EmployeeInfos.FirstOrDefault(e => e.Id == employeeInfoId);
            if (employee.Chief)
            {
                accounts = context.Accounts.Where(a => a.CompanyId == employee.CompanyId).ToList();
                return new SelectList(accounts, "Id", "Number");
            }

            accounts = context.EmployeeAccounts
                .Where(ea => ea.EmployeeId == employeeInfoId && ea.RightOfCreate)
                .Select(a => context.Accounts.Include(ac => ac.Currency).FirstOrDefault(acc => acc.Id == a.AccountId))
                .ToList();
            return new SelectList(accounts, "Id", "Number");
        }

        public SelectList GetBankList()
        {
            return new SelectList(context.Banks.Include(b => b.BankInfo).ToList(), "Id", "BankInfo.BankName");
        }

        public SelectList GetPayemntCodeList()
        {
            List<PaymentСode> paymentCode = context.PaymentСodies.ToList();
            List<PaymentСode> paymentList = paymentCode;
            for (int i = 0; i < paymentCode.Count(); i++)
            {
                paymentList[i].Code = string.Format("{0} {1}", paymentCode[i].Code, paymentCode[i].PaymentCodeName);

            }

            return new SelectList(paymentList, "Id", "Code");
        }

        public SelectList GetIntervalTypes()
        {
            List<IntervalType> intervalTypes = context.IntervalTypes.ToList();
            foreach (var intervalType in intervalTypes)
            {
                switch (intervalType.IntervalCode)
                {
                    case (int)IntervalTypesEnum.OnceADay: intervalType.IntervalName = "Каждый день"; break;
                    case (int)IntervalTypesEnum.OnceAWeek: intervalType.IntervalName = "Раз в неделю"; break;
                    case (int)IntervalTypesEnum.OnceInTwoWeeks: intervalType.IntervalName = "Раз в две недели"; break;
                    case (int)IntervalTypesEnum.OnceAMonth: intervalType.IntervalName = "Раз в месяц"; break;
                    case (int)IntervalTypesEnum.OnceAQuarter: intervalType.IntervalName = "Раз в три месяца"; break;
                    case (int)IntervalTypesEnum.OnceAHalfYear: intervalType.IntervalName = "Раз в полгода"; break;
                    case (int)IntervalTypesEnum.OnceAYear: intervalType.IntervalName = "Раз в год"; break;
                }

            }

            SelectList selectList = new SelectList(intervalTypes, "IntervalCode", "IntervalName");
            return selectList;
        }

        public SelectList GetTypeOfTransfers()
        {
            SelectList selectList = new SelectList(context.TypeOfTransfers.ToList(), "Id", "Name");
            return selectList;
        }
    }
}



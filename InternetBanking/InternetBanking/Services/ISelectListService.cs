using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.Services
{
    public interface ISelectListService
    {
        SelectList GetCountries();
        SelectList GetLegalForms();
        SelectList GetPropertyTypes();
        SelectList GetResidencies();
        SelectList GetTaxInspections();
        SelectList GetCurrencies();
        SelectList GetLimits();
        SelectList GetUserAccounts(int userInfoId);
        AddCompanyViewModel GetCompaniesSelectList(AddCompanyViewModel company);
        SelectList GetTypeOfDocuments();
        SelectList GetEmployeeAccounts(int employeeInfoId);
        SelectList GetBankList();
        SelectList GetPayemntCodeList();
        SelectList GetCompanyAccounts(int companyId);
        SelectList GetIntervalTypes();
        SelectList GetTypeOfTransfers();
    }
}

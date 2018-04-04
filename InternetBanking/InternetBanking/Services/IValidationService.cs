using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternetBanking.Services
{
    public interface IValidationService
    {
        ModelStateDictionary ValidateAddCompanyViewModel(AddCompanyViewModel model, ModelStateDictionary ModelState);
        ModelStateDictionary ValidateCompanyEditViewModel(CompanyEditViewModel model, ModelStateDictionary ModelState);
        ModelStateDictionary ValidateRegisterPersonViewModel(RegisterPersonViewModel model, ModelStateDictionary ModelState);
        Task<ModelStateDictionary> ValidateInnerTransfer(InnerTransferViewModel model, User user, Account sender ,ModelStateDictionary ModelState);
        Task<ModelStateDictionary> ValidateInnerTransferTemplate(InnerTransferViewModel model, User user, ModelStateDictionary ModelState);
        Task<ModelStateDictionary> ValidateInterTransfer(InnerTransferViewModel model, User user, Account receiver,
            ModelStateDictionary ModelState);
        ModelStateDictionary ValidateLimitCreating(LimitInfoViewModel model, ModelStateDictionary ModelState); 
        ModelStateDictionary ValidateLimitEditing(EditLimitViewModel model, ModelStateDictionary ModelState); 
        ModelStateDictionary ValidateRateCreatingAndEditing(ExchangeRateViewModel model, ModelStateDictionary ModelState);
        ModelStateDictionary ValidateBankCreating(CreateBankViewModel model, ModelStateDictionary ModelState);
        ModelStateDictionary ValidateBankEditing(EditBankViewModel model, ModelStateDictionary ModelState);

       Task<ModelStateDictionary> ValidateConvertUserCurrency(CurrencyConversionViewModel model, Account sender, ModelStateDictionary ModelState,User user);
      
        ModelStateDictionary ValidatePaymentSchedule(TemplateViewModel model, ModelStateDictionary ModelState);

        ModelStateDictionary ValidateAddPaymentSchedule(PaymentScheduleViewModel model, ModelStateDictionary ModelState);
        

    }
}

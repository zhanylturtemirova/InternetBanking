using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Controllers;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace InternetBanking.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IUserService userService;
        private readonly ITransferService transferService;
        private readonly IAccountService accountService;
        private readonly IStringLocalizer<ValidationService> localizer;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ILimitService limitService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IBankService bankService;
      

        public ValidationService(IUserService userService, ITransferService transferService, IAccountService accountService, IStringLocalizer<ValidationService> localizer, IEmployeeService employeeService, ICompanyService companyService, IHostingEnvironment appEnvironment, ILimitService limitService, IExchangeRateService exchangeRateService, IBankService bankService)
        {
            this.userService = userService;
            this.transferService = transferService;
            this.accountService = accountService;
            this.localizer = localizer;
            this.employeeService = employeeService;
            this.companyService = companyService;
            _appEnvironment = appEnvironment;
            this.limitService = limitService;
            this.exchangeRateService = exchangeRateService;
            this.bankService = bankService;
        }

        public ModelStateDictionary ValidateAddCompanyViewModel(AddCompanyViewModel model, ModelStateDictionary ModelState)
        {
            DateTime dateOfInitialRegistration = new DateTime();
            if (DateTime.TryParse(model.DateOfInitialRegistration, out dateOfInitialRegistration))
            {
                if (dateOfInitialRegistration > DateTime.Now)
                {
                    ModelState.AddModelError("DateOfInitialRegistration", "Вы указали дату из будущего!");
                }
                if (dateOfInitialRegistration < DateTime.Parse("01.01.1991 00:00:00"))
                {
                    ModelState.AddModelError("DateOfInitialRegistration", "Вы указали неверную дату!");
                }
            }
            else
            {
                ModelState.AddModelError("DateOfInitialRegistration", "*Введите дату!");
            }

            DateTime dateOfRegistrationMinistryJustice = new DateTime();
            if (DateTime.TryParse(model.DateOfRegistrationMinistryJustice, out dateOfRegistrationMinistryJustice))
            {
                if (dateOfRegistrationMinistryJustice > DateTime.Now)
                {
                    ModelState.AddModelError("DateOfRegistrationMinistryJustice", "Вы указали дату из будущего!");
                }
                if (dateOfRegistrationMinistryJustice < DateTime.Parse("01.01.1991 00:00:00"))
                {
                    ModelState.AddModelError("DateOfRegistrationMinistryJustice", "Вы указали неверную дату!");
                }
            }
            else
            {
                ModelState.AddModelError("DateOfInitialRegistration", "*Введите дату!");
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateCompanyEditViewModel(CompanyEditViewModel model, ModelStateDictionary ModelState)
        {
            if (model.RegistrationData.DateOfInitialRegistration > DateTime.Now)
            {
                ModelState.AddModelError("RegistrationData.DateOfInitialRegistration", "Вы ввели дату из будущего!");
            }
            if (model.RegistrationData.DateOfRegistrationMinistryJustice > DateTime.Now)
            {
                ModelState.AddModelError("RegistrationData.DateOfRegistrationMinistryJustice", "Вы ввели дату из будущего!");
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateRegisterPersonViewModel(RegisterPersonViewModel model, ModelStateDictionary ModelState)
        {
            DateTime dateOfExtradition = new DateTime();
            DateTime validaty = new DateTime();
            DateTime birthDay = new DateTime();
            if (DateTime.TryParse(model.PassportInfo.DateofExtradition, out dateOfExtradition))
            {
                if (DateTime.Now < dateOfExtradition || dateOfExtradition.AddYears(10) < DateTime.Now)
                {
                    ModelState.AddModelError("PassportInfo.DateofExtradition", "Дата выдачи не может быть из будущего или прошлого");
                }
            }
            else
            {
                ModelState.AddModelError("PassportInfo.DateofExtradition", "*Введите дату!");
            }
            if (DateTime.TryParse(model.PassportInfo.Validaty, out validaty))
            {
                if (validaty <= dateOfExtradition)
                {
                    ModelState.AddModelError("PassportInfo.Validaty", "Срок действия не может быть раньше даты выдачи");
                }
                if (validaty > dateOfExtradition.AddYears(10))
                {
                    ModelState.AddModelError("PassportInfo.Validaty", "Срок действия не может превышать 10 лет");
                }
            }
            else
            {
                ModelState.AddModelError("PassportInfo.Validaty", "*Введите дату!");
            }
            if (DateTime.TryParse(model.UserInfo.BirthDay, out birthDay))
            {
                if (DateTime.Now < birthDay || birthDay.AddYears(118) < DateTime.Now)
                {
                    ModelState.AddModelError("UserInfo.BirthDay", "Дата рождения не может быть из будущего или прошлого");
                }
            }
            else
            {
                ModelState.AddModelError("UserInfo.BirthDay", "*Введите дату!");
            }
            return ModelState;
        }

        public async Task<ModelStateDictionary> ValidateInnerTransfer(InnerTransferViewModel model, User user, Account sender, ModelStateDictionary ModelState)
        {
             decimal amount = 0;             
            if (model.AccountSenderId == null)
            {
                ModelState.AddModelError("AccountSenderId", localizer["AccountSenderIdValidation"]);
            }
            else
            {
                EmployeeAccount Limit = accountService.FindEmployeeAccountByUserIdAndAccountId(user.Id, model.AccountSenderId.Value);
                if (sender.Locked)
                {
                    ModelState.AddModelError("AccountSenderId", "*Счет заблокирован");
                }
                else if (!await accountService.IsAccountExist(model.ReceiverAccountNumber))
                {
                    ModelState.AddModelError("ReceiverAccountNumber", localizer["ReceiverAccountNumberExistVlidation"]);

                }
                else if (await accountService.IsAccountExist(model.ReceiverAccountNumber) && await accountService.IsAccountSenderNotReceiver(model.ReceiverAccountNumber, user))
                {
                    ModelState.AddModelError("ReceiverAccountNumber", localizer["IsAccountSenderNotReceiverValidation"]);
                }
                else if (!await accountService.IsUserHaveRightsOnAccount(user, model.AccountSenderId.Value))
                {
                    ModelState.AddModelError("AccountSenderId", "У вас нет прав на совершение данного перевода.");
                }
                else if (await accountService.IsAccountExist(model.ReceiverAccountNumber) && await accountService.CompareAccountsCurrencies(model.ReceiverAccountNumber, (int)model.AccountSenderId))
                {
                    ModelState.AddModelError("ReceiverAccountNumber", localizer["CurrencyValidation"]);
                }
                if (!accountService.AmountTryParse(model.Amount, out amount))
                {
                    ModelState.AddModelError("Amount", localizer["AmountFormatValidation"]);
                }
                else if (amount <= 0)
                {
                    ModelState.AddModelError("Amount", localizer["AmountNotNull"]);
                }
                else if (!await accountService.IsBalanceEnough((int)model.AccountSenderId, amount))
                {
                    ModelState.AddModelError("Amount", localizer["IsBalanceEnoughValidation"]);
                }
                else if (Limit != null)
                {
                    if (Limit.limit.LimitAmount < amount)
                    {
                        ModelState.AddModelError("Amount", string.Format("{0} {1}", localizer["ExceededTheLimit"], Limit.limit.LimitAmount));
                    }
                }

            }
            return ModelState;
        }

        public async Task<ModelStateDictionary> ValidateInnerTransferTemplate(InnerTransferViewModel model, User user, ModelStateDictionary ModelState)
        {
            decimal amount = 0;

            if (model.AccountSenderId == null)
            {
                ModelState.AddModelError("Transfer.AccountSenderId", localizer["AccountSenderIdValidation"]);
            }
            else
            {
                EmployeeAccount Limit = accountService.FindEmployeeAccountByUserIdAndAccountId(user.Id, model.AccountSenderId.Value);
                if (!await accountService.IsAccountExist(model.ReceiverAccountNumber))
                {
                    ModelState.AddModelError("Transfer.ReceiverAccountNumber", localizer["ReceiverAccountNumberExistVlidation"]);

                }
                else if (await accountService.IsAccountExist(model.ReceiverAccountNumber) && await accountService.IsAccountSenderNotReceiver(model.ReceiverAccountNumber, user))
                {
                    ModelState.AddModelError("Transfer.ReceiverAccountNumber", localizer["IsAccountSenderNotReceiverValidation"]);
                }
                else if (!await accountService.IsUserHaveRightsOnAccount(user, model.AccountSenderId.Value))
                {
                    ModelState.AddModelError("Transfer.AccountSenderId", "У вас нет прав на совершение данного перевода.");
                }
                else if (await accountService.IsAccountExist(model.ReceiverAccountNumber) && await accountService.CompareAccountsCurrencies(model.ReceiverAccountNumber, (int)model.AccountSenderId))
                {
                    ModelState.AddModelError("Transfer.ReceiverAccountNumber", localizer["CurrencyValidation"]);
                }
                if (!accountService.AmountTryParse(model.Amount, out amount))
                {
                    ModelState.AddModelError("Transfer.Amount", localizer["AmountFormatValidation"]);
                }
                else if (amount <= 0)
                {
                    ModelState.AddModelError("Transfer.Amount", localizer["AmountNotNull"]);
                }
                else if (!await accountService.IsBalanceEnough((int)model.AccountSenderId, amount))
                {
                    ModelState.AddModelError("Transfer.Amount", localizer["IsBalanceEnoughValidation"]);
                }
                else if (Limit != null)
                {
                    if (Limit.limit.LimitAmount < amount)
                    {
                        ModelState.AddModelError("Transfer.Amount", string.Format("{0} {1}", localizer["ExceededTheLimit"], Limit.limit.LimitAmount));
                    }
                }

            }
            return ModelState;
        }

        public async Task<ModelStateDictionary> ValidateInterTransfer(InnerTransferViewModel model, User user, Account receiver,
            ModelStateDictionary ModelState)
        {
            decimal amount = 0;

            if (model.AccountSenderId == null)
            {
                ModelState.AddModelError("Transfer.Transfer.AccountSenderId", localizer["AccountSenderIdValidation"]);
            }
            else
            {
                EmployeeAccount Limit = accountService.FindEmployeeAccountByUserIdAndAccountId(user.Id, model.AccountSenderId.Value);
                if (!await accountService.IsUserHaveRightsOnAccount(user, model.AccountSenderId.Value))
                {
                    ModelState.AddModelError("Transfer.Transfer.AccountSenderId", "У вас нет прав на совершение данного перевода.");
                }
                else if (await accountService.IsAccountExist(receiver.Number) && await accountService.CompareAccountsCurrencies(receiver.Number, (int)model.AccountSenderId))
                {
                    ModelState.AddModelError("Transfer.Transfer.AccountSenderId", localizer["*Межбанковский перевод провидиться в национальной валюте"]);
                }
                if (!accountService.AmountTryParse(model.Amount, out amount))
                {
                    ModelState.AddModelError("Transfer.Transfer.Amount", localizer["AmountFormatValidation"]);
                }
                else if (amount <= 0)
                {
                    ModelState.AddModelError("Transfer.Transfer.Amount", localizer["AmountNotNull"]);
                }
                else if (!await accountService.IsBalanceEnough((int)model.AccountSenderId, amount))
                {
                    ModelState.AddModelError("Transfer.Transfer.Amount", localizer["IsBalanceEnoughValidation"]);
                }
                else if (Limit.limit.LimitAmount < amount)
                {
                    ModelState.AddModelError("Transfer.Transfer.Amount", string.Format("{0} {1}", localizer["ExceededTheLimit"], Limit.limit.LimitAmount));
                }
            }
                return ModelState;
        }

        public ModelStateDictionary ValidateLimitCreating(LimitInfoViewModel model, ModelStateDictionary ModelState)
        {
            Limit limit = limitService.FindLimitName(model.LimitName);
            decimal amount = 0;
            if (limit != null)
            {
                ModelState.AddModelError("LimitName", "Такой лимит уже существует");
            }
            if (!limitService.AmountTryParse(model.LimitAmount, out amount))
            {
                ModelState.AddModelError("LimitAmount", "Введите число в формате xxx,xx");
            }
            else if (amount <= 0)
            {
                ModelState.AddModelError("LimitAmount", "Сумма лимита должна быть больше нуля");
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateLimitEditing(EditLimitViewModel model, ModelStateDictionary ModelState)
        {
            Limit limit = limitService.FindLimitName(model.LimitInfo.LimitName);
            decimal amount = 0;
            if (limit != null && limit.Id != model.Limit.Id)
            {
                ModelState.AddModelError("LimitInfo.LimitName", "Такой лимит уже существует");
            }
            if (!limitService.AmountTryParse(model.LimitInfo.LimitAmount, out amount))
            {
                ModelState.AddModelError("LimitInfo.LimitAmount", "Введите число в формате xxx,xx");
            }
            else if (amount <= 0)
            {
                ModelState.AddModelError("LimitInfo.LimitAmount", "Сумма лимита должна быть больше нуля");
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateRateCreatingAndEditing (ExchangeRateViewModel model, ModelStateDictionary ModelState)
        {
            decimal rate = 0;

            if (!exchangeRateService.RateTryParse(model.RateForPurchaise, out rate))
            {
                ModelState.AddModelError("Rate", localizer["RateFormatValidation"]);
            }
           else if (!exchangeRateService.RateTryParse(model.RateForSale, out rate))
            {
                ModelState.AddModelError("Rate", localizer["RateFormatValidation"]);
            }
            else if (rate <= 0)
            {
                ModelState.AddModelError("Rate", localizer["RateNotNull"]);
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateBankCreating(CreateBankViewModel model, ModelStateDictionary ModelState)
        {
            Bank bank = bankService.GetBankBik(model.BIK);
            if (bank != null)
            {
                ModelState.AddModelError("BIK", "Такой БИК уже существует");
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateBankEditing(EditBankViewModel model, ModelStateDictionary ModelState)
        {
            Bank bank = bankService.GetBankBik(model.BankInfo.BIK);
            if (bank != null && bank.Id != model.Bank.Id)
            {
                ModelState.AddModelError("BankInfo.BIK", "Такой БИК уже существует");
            }
            return ModelState;
        }

       


        public async Task<ModelStateDictionary> ValidateConvertUserCurrency(CurrencyConversionViewModel model, Account sender, ModelStateDictionary ModelState, User user)
        {

            EmployeeAccount Limit = accountService.FindEmployeeAccountByUserIdAndAccountId(user.Id, model.AccountSenderId);
            Account receiverAccount = accountService.FindAccountById(model.AccountReceiverId).Result;
            Account senderAccount = accountService.FindAccountById(model.AccountSenderId).Result;
            decimal amountSend = 0;
            if (senderAccount == null)
            {
                ModelState.AddModelError("AccountSenderId", localizer["AccountSenderIdValidation"]);
            }
            else if (receiverAccount == null)
            {
                ModelState.AddModelError("AccountReceiverId", localizer["AccountReceiverIdValidation"]);

            }
            else
            {
                if (sender.Locked)
                {
                    ModelState.AddModelError("AccountSenderId", "*Счет заблокирован");
                }
                else
                if (!await accountService.IsAccountExist(receiverAccount.Number))
                {
                    ModelState.AddModelError("AccountReceiverId", localizer["ReceiverAccountNumberExistVlidation"]);

                }
                else if (!await accountService.IsAccountSenderNotReceiver(receiverAccount.Number, user))
                {
                    ModelState.AddModelError("AccountSenderId", localizer["IsAccountSenderNotReceiverValidation"]);
                }
                else if (senderAccount.Id == receiverAccount.Id)
                {
                    ModelState.AddModelError("AccountSenderId", localizer["AccountReceiverValidation"]);
                }
                else if (!await accountService.IsUserHaveRightsOnAccount(user, senderAccount.Id))
                {
                    ModelState.AddModelError("AccountSenderId", localizer["SenderRightsValidation"]);
                }
                if (!accountService.AmountTryParse(model.AmountSend, out amountSend))
                {
                    ModelState.AddModelError("AmountSend", localizer["AmountFormatValidation"]);
                }
                else if (amountSend <= 0)
                {
                    ModelState.AddModelError("AmountSend", localizer["AmountNotNull"]);
                }
                else if (!await accountService.IsBalanceEnough(senderAccount.Id, amountSend))
                {
                    ModelState.AddModelError("AmountSend", localizer["IsBalanceEnoughValidation"]);
                }
                else if (Limit.limit.LimitAmount < amountSend)
                {
                    ModelState.AddModelError("AmountSend",
                        string.Format("{0} {1}", localizer["ExceededTheLimit"], Limit.limit.LimitAmount));
                }
            }


            return ModelState;
        }


        public ModelStateDictionary ValidatePaymentSchedule(TemplateViewModel model, ModelStateDictionary ModelState)
        {
            if (model.IsSetSchedule)
            {
                ValidateAddPaymentSchedule(model.PaymentScheduleViewModel, ModelState);
            }
            return ModelState;
        }

        public ModelStateDictionary ValidateAddPaymentSchedule(PaymentScheduleViewModel model, ModelStateDictionary ModelState)
        {
            DateTime finishDate;
            if (model.IntervalCode == 0)
            {
                ModelState.AddModelError("IntervalCode", "*Выберите интервал проведения платежа!");
            }
            if (model.DateStart < DateTime.Now.Date.AddDays(1))
            {
                ModelState.AddModelError("DateStart", string.Format("*Первоначальное выполнение платежа доступно только c {0}", DateTime.Now.AddDays(1).ToString("d")));
            }
            if (DateTime.TryParse(model.DateEnd, out finishDate) && finishDate < model.DateStart)
            {
                ModelState.AddModelError("DateEnd", "*Дата окончания должна быть больше даты начала!");
            }

            return ModelState;
        }


       /* public ModelStateDictionary ValidateEditPaymentSchedule(PaymentScheduleViewModel model, PaymentSchedule schedule, ModelStateDictionary ModelState)
        {
            if (model.IntervalCode == 0)
            {
                ModelState.AddModelError("IntervalCode", "*Выберите интервал проведения платежа!");
            }
            else
            {
                DateTime finishDate;
                if (model.IntervalCode != schedule.IntervalType.IntervalCode)
                {
                  
                    if (model.DateStart < DateTime.Now.Date.AddDays(1))
                    {
                        ModelState.AddModelError("DateStart",
                            string.Format("*Первоначальное выполнение платежа доступно только c {0}",
                                DateTime.Now.AddDays(1).ToString("d")));
                    }
                    if (DateTime.TryParse(model.DateEnd, out finishDate) && finishDate < model.DateStart)
                    {
                        ModelState.AddModelError("DateEnd", "*Дата окончания должна быть больше даты начала!");
                    }
                }
                else
                {
                    if (model.DateStart != schedule.Start)
                    {
                        if (model.DateStart < DateTime.Now.Date.AddDays(1))
                        {
                            ModelState.AddModelError("DateStart",
                                string.Format("*Первоначальное выполнение платежа доступно только c {0}",
                                    DateTime.Now.AddDays(1).ToString("d")));
                        }
                        if (DateTime.TryParse(model.DateEnd, out finishDate))
                        {
                            if (finishDate < model.DateStart)
                            {
                                ModelState.AddModelError("DateEnd", "*Дата окончания должна быть больше даты начала!");
                            }
                         
                        }
                    }            
                }
            }
            return ModelState;

        }*/
    }
}

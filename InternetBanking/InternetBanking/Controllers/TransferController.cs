using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using InternetBanking.ViewModels.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;


namespace InternetBanking.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        private readonly ISelectListService selectListService;
        private readonly IUserService userService;
        private readonly ITransferService transferService;
        private readonly IAccountService accountService;
        private readonly IStringLocalizer<TransferController> localizer;
        private readonly IHomePagingService pagingService;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IValidationService validationService;
        private readonly IDocumentFormatService documentFormatService;
        private readonly ITemplateService _templateServiceService;
        private readonly ICreatePDFandLoad createPdFandLoad;
        private readonly ICurrencyService currencyService;
        private readonly IHostingEnvironment _hostingEnvironment;
   

        public TransferController(ISelectListService selectListService, IUserService userService, 
            ITransferService transferService, IAccountService accountService, 
            IStringLocalizer<TransferController> localizer, IHomePagingService pagingService,
            IEmployeeService employeeService, ICompanyService companyService,
            IExchangeRateService exchangeRateService, IValidationService validationService, 
            IDocumentFormatService documentFormatService, ITemplateService templateServiceService, 
            ICreatePDFandLoad createPdFandLoad, ICurrencyService currencyService, IHostingEnvironment _hostingEnvironment)
        {
            this.selectListService = selectListService;
            this.userService = userService;
            this.transferService = transferService;
            this.accountService = accountService;
            this.localizer = localizer;
            this.pagingService = pagingService;
            this.employeeService = employeeService;
            this.companyService = companyService;
            this.exchangeRateService = exchangeRateService;
            this.validationService = validationService;
            this.documentFormatService = documentFormatService;
            this._templateServiceService = templateServiceService;
            this.createPdFandLoad = createPdFandLoad;
            this.currencyService = currencyService;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public async Task<IActionResult> InnerTransfer()
        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            InnerTransferViewModel transfer = new InnerTransferViewModel();          
            InnerTransferViewModel model = transferService.GetMethodInnerTransfer(user, transfer);
            
            
            return View(model);
        }
        [HttpGet]
        public IActionResult InnerTransferTemplate(int templateId)
        {
            Template template = _templateServiceService.FindTemplateById(templateId);
            InnerTransferViewModel model = new InnerTransferViewModel
            {
                AccountSenderId = template.AccountSenderId,
                ReceiverAccountNumber = template.AccountReceiver.Number,
                Amount = template.Amount.ToString(),
                Comment = template.Comment,
                TemplateId = template.Id,
                Template = template,
                SaveInTempalte = false

            };
            if (template.UserInfo != null)
            {
                model.UserAccounts = selectListService.GetUserAccounts(template.UserInfo.Id);
            }
            if (template.Company != null)
            {
                model.UserAccounts = selectListService.GetCompanyAccounts(template.Company.Id);
            }
            return View("InnerTransfer",model);

        }

        [HttpPost]
        public async Task<IActionResult> InnerTransfer(InnerTransferViewModel model)
        {
            decimal amount = 0;          
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            Account sender = accountService.FindAccountById((int)model.AccountSenderId).Result;
            await validationService.ValidateInnerTransfer(model, user, sender, ModelState);
            string comment = "Внутрибанковский платеж - " + model.Comment;
            model.Comment = comment;
            if (ModelState.IsValid)
            {
                amount = decimal.Parse(model.Amount);
              Account receiver = accountService.FindAccountByNumber(model.ReceiverAccountNumber).Result;
                //decimal amountReceive;
                //accountService.AmountTryParse(model.AmountReceive, out amountReceive);
                    InnerTransfer transfer = await
                        transferService.CreateInnerTransfer(sender, receiver, amount, model.Comment,null, null, null);
                    transferService.AddTransfer(transfer);

                if (model.SaveInTempalte)
                {
                    Template template = _templateServiceService.SaveTemplate(transfer, user);
                    return RedirectToAction("TemplateSave", "Template" , new { tempalteId = template.Id });
                }
                return RedirectToAction("Transfer", "Transfer");
            }
            model = transferService.GetMethodInnerTransfer(user, model);
            Template templates = _templateServiceService.FindTemplateById(model.TemplateId);
            if (templates != null)
            {
                model.Template = templates;
            }
            return View(model);
        }

        public IActionResult Transfer()
        {
            return View();
        }


        [HttpGet]
        public IActionResult InterTransferTemplate(int templateId)
        {
            Template template = _templateServiceService.FindTemplateById(templateId);
            string amount = String.Format("{0:0.00}", template.Amount);
            InterBankTransferViewModel model = new InterBankTransferViewModel
            {
                Banks = selectListService.GetBankList(),
                BankId = template.BankId,
                PaymentCode = selectListService.GetPayemntCodeList(),
                PaymentCodeId = template.PaymentCodeId,
                ReciverName = template.ReciverName,
                Transfer = new InnerTransferViewModel
                {
                    AccountSenderId = template.AccountSenderId,
                    ReceiverAccountNumber = template.AccountNumber,
                    Amount = amount,
                    Comment = template.Comment,
                    TemplateId = template.Id,
                    Template = template,
                    SaveInTempalte = false
                }
            };
            if (template.UserInfo != null)
            {
                model.Transfer.UserAccounts = selectListService.GetUserAccounts(template.UserInfo.Id);
            }
            if (template.Company != null)
            {
                model.Transfer.UserAccounts = selectListService.GetCompanyAccounts(template.Company.Id);
            }
            return View("InterTransfer" , model);
        }
        [HttpGet]
        public IActionResult InterTransfer()
        {
           User user = userService.FindUserByName(HttpContext.User.Identity.Name);
           InterBankTransferViewModel model = new InterBankTransferViewModel{ Banks = selectListService.GetBankList(), PaymentCode = selectListService.GetPayemntCodeList(), Transfer = new InnerTransferViewModel()};
            model.Transfer = transferService.GetMethodInnerTransfer(user, model.Transfer);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> InterTransfer(InterBankTransferViewModel model)
        {
            decimal amount = 0;
            Account receiver =accountService.OurBankAccount();
            Account sender = accountService.FindAccountById((int)model.Transfer.AccountSenderId).Result;
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            EmployeeAccount Limit = new EmployeeAccount();
            string comment = "Межбанковский платеж - " + model.Transfer.Comment;
            model.Transfer.Comment = comment;
            if (model.Transfer.AccountSenderId == null)
            {
                ModelState.AddModelError("Transfer.AccountSenderId", localizer["AccountSenderIdValidation"]);
            }
            else
            {
               Limit= accountService.FindEmployeeAccountByUserIdAndAccountId(user.Id, model.Transfer.AccountSenderId.Value);
                if (sender.Locked)
                {
                    ModelState.AddModelError("Transfer.AccountSenderId", "*Счет заблокирован");
                }
                else if (!await accountService.IsUserHaveRightsOnAccount(user, model.Transfer.AccountSenderId.Value))
                {
                    ModelState.AddModelError("Transfer.AccountSenderId", "У вас нет прав на совершение данного перевода.");
                }
                else if (await accountService.IsAccountExist(receiver.Number) && await accountService.CompareAccountsCurrencies(receiver.Number, (int)model.Transfer.AccountSenderId))
                {
                    ModelState.AddModelError("Transfer.AccountSenderId", localizer["*Межбанковский перевод провидиться в национальной валюте"]);
                }
                if (!accountService.AmountTryParse(model.Transfer.Amount, out amount))
                {
                    ModelState.AddModelError("Transfer.Amount", localizer["AmountFormatValidation"]);
                }
                else if (amount <= 0)
                {
                    ModelState.AddModelError("Transfer.Amount", localizer["AmountNotNull"]);
                }
                else if (!await accountService.IsBalanceEnough((int)model.Transfer.AccountSenderId, amount))
                {
                    ModelState.AddModelError("Transfer.Amount", localizer["IsBalanceEnoughValidation"]);
                }
                else if (Limit.limit.LimitAmount < amount)
                {
                    ModelState.AddModelError("Transfer.Amount", string.Format("{0} {1}", localizer["ExceededTheLimit"], Limit.limit.LimitAmount));
                }
            }
            if (ModelState.IsValid)
            {
               InnerTransfer transfer = await
                    transferService.CreateInnerTransfer(sender, receiver, amount, model.Transfer.Comment, null, null, null);
                transferService.AddTransfer(transfer);
                InterBankTransfer interTransfer =transferService.CreateInterBankTransfer(model, transfer);

                if (model.Transfer.SaveInTempalte)
                {
                    Template template = _templateServiceService.SaveTemplateInterTransfer(interTransfer, user);
                    return RedirectToAction("TemplateSave", "Template", new { tempalteId = template.Id });
                }
                return RedirectToAction("Transfer", "Transfer");
            }

            Template templates = _templateServiceService.FindTemplateById(model.Transfer.TemplateId);
            if (templates != null)
            {
                model.Transfer.Template = templates;
            }
            model.Banks = selectListService.GetBankList();
            model.PaymentCode = selectListService.GetPayemntCodeList();
            model.Transfer = transferService.GetMethodInnerTransfer(user, model.Transfer);
           return View(model);
        }

        [HttpGet]
        public IActionResult AddMoneyUserAccount(int accountId)
        {
            Account account = accountService.FindAccountById(accountId).Result;
            AddMoneyViewModel model = new AddMoneyViewModel { Account = account};
          
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMoneyUserAccount(AddMoneyViewModel model)
        {
            Account sender = null;
            Account receiver = accountService.FindAccountById(model.Account.Id).Result;
            string comment = "Пополнение";
            decimal amount = 0;
               
                if (!accountService.AmountTryParse(model.Amount, out amount))
                {
                    ModelState.AddModelError("Amount", localizer["AmountFormatValidation"]);
                }
                else if (amount <= 0)
                {
                    ModelState.AddModelError("Amount", localizer["AmountNotNull"]);
                }

            if (ModelState.IsValid)
            {

                InnerTransfer transfer = await
                    transferService.CreateInnerTransfer(sender, receiver, amount, comment,null, null, null);
                transferService.AddTransfer(transfer);
                if (receiver.UserInfoId!= null)
                {
                    return RedirectToAction("UserAccounts", "Account", new { userId = receiver.UserInfo.User.Id});
                }
                else if (receiver.CompanyId != null)
                {
                    return RedirectToAction("Index", "Account", new { companyId = receiver.CompanyId });
                }

            }
            model.Account = receiver;
            return View(model);
        }

        public async Task<IActionResult> GetTransfers(string section = "all", int page = 1)
        {
            
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            EmployeeInfo employee = await employeeService.GetEmployeeInfoByUserId(user.Id);
         
            IQueryable<ConfirmTransferViewModel> transfers;
            if (section == "not_confirmed")
            {
                transfers =
                     transferService.GetNotConfirmedTransferViewModelsByCompanyId(user);
            }
            else
            {

                transfers = transferService.GetAllTransferViewModelsByCompanyId(user);
            }
            PagedObject<ConfirmTransferViewModel> pagedObject = await pagingService.DoPage<ConfirmTransferViewModel>(transfers, page);
            if (employee != null)
            {
                foreach (var confirmTransferViewModel in pagedObject.Objects)
                {
                    confirmTransferViewModel.IsUserHaveRightOfConfirm =
                        transferService.GetEmployeeRightOfConfirm(employee,
                            confirmTransferViewModel.Transfer.AccountSender);
                }
            }
            else
            {
                UserInfo userInfo = await userService.FindUserInfoByUserId(user.Id);
                foreach (var confirmTransferViewModel in pagedObject.Objects)
                {
                    if (confirmTransferViewModel.Transfer.AccountSender != null)
                    {
                        if (confirmTransferViewModel.Transfer.AccountSender.UserInfoId == userInfo.Id)
                        {

                            confirmTransferViewModel.IsUserHaveRightOfConfirm = true;
                        }
                        else
                        {
                            confirmTransferViewModel.IsUserHaveRightOfConfirm = false;
                        }
                    }
                    confirmTransferViewModel.IsUserHaveRightOfConfirm = false;

                }
            }
           
            PagingViewModel<ConfirmTransferViewModel> model = new PagingViewModel<ConfirmTransferViewModel>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            ViewBag.Section = section;
        
            return View(model);
        }
       
        public async Task<IActionResult> Confirm(int transferId)
        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            EmployeeInfo employee = await employeeService.GetEmployeeInfoByUserId(user.Id);
            InnerTransfer transfer = await transferService.FindTransferById(transferId);
            if (transfer == null)
            {
                return Json(new { message = "Такого платежа не существует!!!", state = false });
            }
            Account account = await accountService.FindAccountById(transfer.AccountSenderId.Value);
            if (!transferService.GetEmployeeRightOfConfirm(employee, account))
            {
                return Json(new { message = "Не удалось подтвердить платеж. Ошибка доступа.", state = false });
            }
            if (!transfer.TransferState.IsEqual(TransferStatesEnum.NotConfirmed))
            {
                return Json(new { message = "Платеж уже подтвержден.", state = true });
            }
            if (!await accountService.IsBalanceEnough(transfer.AccountSenderId.Value, transfer.Amount))
            {
                return Json(new {message = "Не удалось подтвердить платеж.Недостаточно средств.", state = false });
            }

            await transferService.ConfirmTransfer(transfer);
            return Json(new {message = "Платеж подтвержден", state = true});
        } 
          
        public async Task<IActionResult> Reject(int transferId)

        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            EmployeeInfo employee = await employeeService.GetEmployeeInfoByUserId(user.Id);
            InnerTransfer transfer = await transferService.FindTransferById(transferId);
            if (transfer == null)
            {
                return Json(new { message = "Такого платежа не существует!!!", state = false });
            }
            Account account = await accountService.FindAccountById(transfer.AccountSenderId.Value);
            if (!transferService.GetEmployeeRightOfConfirm(employee, account))
            {
                return Json(new { message = "Не удалось подтвердить платеж. Ошибка доступа.", state = false });
            }
            if (!transfer.TransferState.IsEqual(TransferStatesEnum.NotConfirmed))
            {
                return Json(new { message = "Платеж уже подтвержден.", state = true });
            }

            await transferService.CancelTransfer(transferId);
            return Json(new {message = "Платеж отменен", state = true});
        }

        public async Task<IActionResult> UpdateTransferViewModel(int transferId)
        {
            InnerTransfer transfer = await transferService.FindTransferById(transferId);
            string state = string.Empty;
            if (transfer.TransferState.IsEqual(TransferStatesEnum.NotConfirmed))
            {
                state = "Не подтвержден";
            }
            if (transfer.TransferState.IsEqual(TransferStatesEnum.Confirmed))
            {
                state = "Завершен";
            }
            if (transfer.TransferState.IsEqual(TransferStatesEnum.Canceled))
            {
                state = "Отменен";
            }
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            EmployeeInfo employee = await employeeService.GetEmployeeInfoByUserId(user.Id);
            bool actions = transferService.GetEmployeeRightOfConfirm(employee, transfer.AccountSender) &&
                           transfer.TransferState.IsEqual(TransferStatesEnum.NotConfirmed);
            string date = transfer.TransferDate.ToString();

            return Json(new
            {
                senderAccount = transfer.AccountSender.Number,
                receiverAccount = transfer.AccountReceiver.Number,
                аmount = transfer.Amount,
                comment = transfer.Comment,
                date = date,
                state = state,
                actions = actions
            });
        }

        [HttpGet]
        public async Task<IActionResult> AccountTransfers(int id, string fromDate, string toDate, string excel, string fullName, int page = 1)
        {
            User user = userService.FindUserByName(User.Identity.Name);
            
           
            List<StatementObjectViewModel> transfers = await transferService.GetAccountTransfers(id);
            if (fromDate != null)
            {
                transfers = transfers.Where(t => t.TransferDate >= DateTime.Parse(fromDate)).ToList();
            }
            if (toDate != null)
            {
                transfers = transfers.Where(t => t.TransferDate <= DateTime.Parse(toDate)).ToList();
            }
            Account account = await accountService.FindAccountById(id);
            List<ExchangeRate> rates = exchangeRateService.GetExchangeRates();  
            AccountTransfersViewModel model = new AccountTransfersViewModel {Transfers = transfers, Account = account, FromDate = fromDate, ToDate = toDate, FullName = fullName, Rates = rates};
            if (excel == "True")
            {
                return ExportToExcel(model);
            }
                
            return View(model);
        }

        public PhysicalFileResult ExportToExcel(AccountTransfersViewModel model)
        {
            string[] file = documentFormatService.CreateExcelStatement(model);
            return PhysicalFile(file[0], file[1], file[2]);
        }

        public async Task<FileResult> DownloadFile(string sender, string receiver, decimal amount,DateTime date, string comment, string state)
        {
           
            User user =userService.FindUserByName(User.Identity.Name);
           UserInfo userInfo = await userService.FindUserInfoByUserId(user.Id);
            EmployeeInfo employee =  await employeeService.GetEmployeeInfoByUserId(user.Id);
            string fullName;
            if (userInfo!= null)
            {
                
                fullName = userInfo.FirstName + " " + userInfo.MiddleName;
            }
            else
            {
                fullName = employee.FirstName + " " + employee.MiddleName;
            }
           
            PdfForExactTransferViewModel model = new PdfForExactTransferViewModel()
            {
              
                AccountSender = sender,
                AccountReceiver = receiver,
                TransferDate = date.ToString(),
                Amount =amount,
                State= state,
                Comment = comment,
                FullName =fullName,
            };
            PdfPTable table =createPdFandLoad.TableForTransfer(model);
            string name =createPdFandLoad.CreatePDF(table);
            
            return GetFile(name);
        }

        public FileResult GetFile(string name)
        {
            string filePath = _hostingEnvironment.ContentRootPath;
            
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(name);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/pdf";
            return File(readStream, mimeType, name);
        }
    }
}
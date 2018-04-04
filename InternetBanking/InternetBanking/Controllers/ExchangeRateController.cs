using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.rtf;
using iTextSharp.text.rtf.parser;


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.EntityFrameworkCore;
using InternetBanking.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Text;

using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using iTextSharp.text.html;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.FileProviders;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;

namespace InternetBanking.Controllers
{
    public class ExchangeRateController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly IAccountService accountService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IStringLocalizer<ExchangeRateController> localizer;
        private readonly IValidationService validationService;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly ISelectListService selectListService;
        private readonly ITransferService transferService;

        private readonly ApplicationContext context;
        private readonly ICompositeViewEngine _viewEngine;

        private readonly IHostingEnvironment _hostingEnvironment;

        private ConvertConversionToDocViewModel _docmodel;
        private ICreatePDFandLoad createPdFandLoad;
        
       

        public ExchangeRateController(ICurrencyService currencyService,
            IAccountService accountService, IExchangeRateService exchangeRateService,
            IStringLocalizer<ExchangeRateController> localizer, IValidationService validationService,
            UserManager<User> userManager, IUserService userService, ISelectListService selectListService,
            ITransferService transferService, IHostingEnvironment hostingEnvironment,
            ApplicationContext context, ICompositeViewEngine _viewEngine, ICreatePDFandLoad createPdFandLoad)
        {
            this.currencyService = currencyService;
            this.accountService = accountService;
            this.exchangeRateService = exchangeRateService;
            this.localizer = localizer;
            this.validationService = validationService;
            this.userManager = userManager;
            this.userService = userService;
            this.selectListService = selectListService;
            this.transferService = transferService;
            this.context = context;
            this._viewEngine = _viewEngine;
            _hostingEnvironment = hostingEnvironment;
            this.createPdFandLoad = createPdFandLoad;
            

        }


        [Authorize]
        public ActionResult Index()
        {

            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            ExchangeRatesListViewModel model = new ExchangeRatesListViewModel()
            {
                exchangeRatesList = exchangeRateService.GetLastExchangeRatesByDate(),
                currencyList = currencyService.GetCurrencies().ToList()
            };

            return View(model);
        }

        public ActionResult Create()
        {
            List<Currency> nativeList = currencyService.GetCurrencies().Where(u => u.IsNativeCurrency).ToList();
            Currency native = nativeList.FirstOrDefault();

            ExchangeRateViewModel model = new ExchangeRateViewModel()
            {
                CurrencyId = -1,
                CurrencyList = currencyService.GetCurrencies().Where(u => u.Name != native.Name).ToList(),

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExchangeRateViewModel model)
        {

            validationService.ValidateRateCreatingAndEditing(model, ModelState);


            decimal rateForSale = 0;
            decimal rateForPurchaise = 0;

            if (!RateTryParse(model.RateForSale, out rateForSale) ||
                !RateTryParse(model.RateForPurchaise, out rateForPurchaise))
            {
                ModelState.AddModelError("RateForSale", localizer["RateFormatValidation"]);
                ModelState.AddModelError("RateForPurchaise", localizer["RateFormatValidation"]);
            }
            else if (rateForSale <= 0 || rateForPurchaise <= 0)
            {
                ModelState.AddModelError("RateForSale", localizer["RateNotNull"]);
                ModelState.AddModelError("RateForPurchaise", localizer["RateNotNull"]);
            }

            if (ModelState.IsValid)
            {

                ExchangeRate exchangeRate = new ExchangeRate()
                {
                    RateForPurchaise = rateForPurchaise, 
                    RateForSale = rateForSale,
                    RateDate = DateTime.Now,
                    CurrencyId = model.CurrencyId,
                    ExchangeRateType = context.ExchangeRateTypes.FirstOrDefault(e=>e.IsEqual(ExchangeRateTypesEnum.Market))
                };

                exchangeRateService.AddExchangeRate(exchangeRate);
                context.SaveChanges();

                return RedirectToAction("Index", "ExchangeRate");
            }

            model.CurrencyList = currencyService.GetCurrencies().ToList();

            return View(model);
        }



        public ActionResult Delete(int id)
        {
            ExchangeRate exchangeRate = exchangeRateService.FindExchangeRateById(id);
            exchangeRateService.RemoveExchangeRate(exchangeRate);
            context.SaveChanges();

            return RedirectToAction("Index", "ExchangeRate");
        }


        public bool RateTryParse(string modelAmount, out decimal amount)
        {
            if (!string.IsNullOrEmpty(modelAmount))
            {
                modelAmount = modelAmount.Replace('.', ',');
            }
            bool tryParse = decimal.TryParse(modelAmount, out amount);
            amount = Math.Round(amount, 2);

            return tryParse;
        }



        [Authorize]
        public async Task<IActionResult> ConvertUserCurrency()
        {
            string userName = String.Empty;
            int userId = 0;
            User user = userService.FindUserByName(User.Identity.Name);
            UserInfo userInfo = userService.FindUserByIdInUserInfo(user.Id, ref userName, ref userId);
            EmployeeInfo employeeInfo = userService.FindUserByIdInCompany(user.Id, ref userName, ref userId);



            CurrencyConversionViewModel currencyConversion = new CurrencyConversionViewModel();

            if (userInfo != null)
            {
                currencyConversion.UserAccounts = selectListService.GetUserAccounts(userInfo.Id);
            }
            if (employeeInfo != null)
            {
                currencyConversion.UserAccounts = selectListService.GetEmployeeAccounts(employeeInfo.Id);
            }

            return View(currencyConversion);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConvertUserCurrency(CurrencyConversionViewModel model)
        {
            User user = userService.FindUserByName(User.Identity.Name);
            Account sender = accountService.FindAccountById(model.AccountSenderId).Result;
            decimal amountSend = 0;
            accountService.AmountTryParse(model.AmountSend, out amountSend);
            await validationService.ValidateConvertUserCurrency(model, sender, ModelState, user);
            if (ModelState.IsValid)
            {
                Account receiver = accountService.FindAccountById(model.AccountReceiverId).Result;
                decimal amountReceive = 0;
                accountService.AmountTryParse(model.AmountReceive, out amountReceive);

                List<ExchangeRate> exchangeRateList = exchangeRateService.GetLastExchangeRatesByDate().ToList();
                ExchangeRate exchangeRate = exchangeRateList.FirstOrDefault(r => r.CurrencyId == receiver.CurrencyId);
                ExchangeRate exchangeRateSecond =
                    exchangeRateList.FirstOrDefault(r => r.CurrencyId == sender.CurrencyId);
                if (exchangeRate == null  )
                {
                    exchangeRate = exchangeRateList.FirstOrDefault(u => u.CurrencyId == sender.CurrencyId);
                }
                if (exchangeRateSecond == null)
                {
                    exchangeRateSecond = exchangeRateList.FirstOrDefault(u => u.CurrencyId == receiver.CurrencyId);
                }
                int exchangeRateId = exchangeRate.Id;
                int exchangeRateIdSecond = exchangeRateSecond.Id;

                InnerTransfer transfer = await
                    transferService.CreateInnerTransfer(sender, receiver, amountSend, "конвертация", amountReceive,
                        exchangeRateId, exchangeRateIdSecond);
                transferService.AddTransfer(transfer);
                ViewBag.successMessage = localizer["ConversionWasSuccessful"];
                ConvertConversionToDocViewModel docModel = new ConvertConversionToDocViewModel()
                {
                    Date = DateTime.Now.ToString(),
                    AccountFromNumber = sender.Number,
                    CurrencyFromName = sender.Currency.Name,
                    AmountSend = amountSend.ToString(),
                    AccountToNumber = receiver.Number,
                    CurrencyToName = receiver.Currency.Name,
                    AmountReceive = amountReceive.ToString(),
                    CurrencyFromRate = exchangeRate.RateForSale.ToString(),
                    CurrencyToRate = exchangeRateSecond.RateForPurchaise.ToString()
                };
                _docmodel = docModel;

               PdfPTable table = createPdFandLoad.TableForConvertion(docModel);
               
                string name = createPdFandLoad.CreatePDF(table);
                ViewBag.FileName = name;
  }
            string userName = String.Empty;
            int userId = 0;
            UserInfo userInfo = userService.FindUserByIdInUserInfo(user.Id, ref userName, ref userId);
            EmployeeInfo employeeInfo = userService.FindUserByIdInCompany(user.Id, ref userName, ref userId);
            if (userInfo != null)
            {
                model.UserAccounts = selectListService.GetUserAccounts(userInfo.Id);
            }
            if (employeeInfo != null)
            {
                model.UserAccounts = selectListService.GetEmployeeAccounts(employeeInfo.Id);
            }
            return View(model);
        }

        public FileResult DownloadFile(string name)
        {
            string filePath = _hostingEnvironment.ContentRootPath;
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(name);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/pdf";
            return File(readStream, mimeType, name);
        }

        public IActionResult  GetCurrencyName(int id)
        {
            Account account =context.Accounts.FirstOrDefault(u=>u.Id==id);

            Currency currency = context.Currencies.FirstOrDefault(u => u.Id == account.CurrencyId);

            string currencyName = currency.Name;

            return Json(currencyName);
        }

        public ActionResult GetExchangeRatePartialView()
        {

            List<ExchangeRate> exchangeRateList = exchangeRateService.GetLastExchangeRatesByDate().ToList();
            var items = new List<ExchangeRateForPartialViewViewModel> { };
            foreach (var rate in exchangeRateList)
            {
                ExchangeRateForPartialViewViewModel item=
                new ExchangeRateForPartialViewViewModel
                {
                    Name = rate.Currency.Name,
                    RateForSale = (decimal) rate.RateForSale,
                    RateForPurchaise = (decimal) rate.RateForPurchaise
                };
                items.Add(item);
            }
            return PartialView("GetExchangeRatePartialView", items);
           
        }
        
       public IActionResult CurrencyConverter(string ammountSend, int accountSenderId, int accountReceiverId)
       {
           decimal ammountReceive=exchangeRateService.CurrencyConverter(ammountSend, accountSenderId, accountReceiverId);
           return Json(ammountReceive);
        }

    }
}
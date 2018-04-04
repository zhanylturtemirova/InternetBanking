using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Paging;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
    public class LimitPaymentCodeController : Controller
    {
        private ApplicationContext context;
        private IHomePagingService pagingService;
        private readonly ILimitService limitService;
        private readonly IPaymentCodeService paymentCodeService;
        private readonly IValidationService validationService;

        public LimitPaymentCodeController(ApplicationContext context, IHomePagingService pagingService, ILimitService limitService, IPaymentCodeService paymentCodeService, IValidationService validationService)
        {
            this.context = context;
            this.pagingService = pagingService;
            this.limitService = limitService;
            this.paymentCodeService = paymentCodeService;
            this.validationService = validationService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Limit> limits = limitService.GetLimitList().OrderBy(l => l.LimitName);
            PagedObject<Limit> pagedObject = await pagingService.DoPage<Limit>(limits, page);

            PagingViewModel<Limit> LimitsPagingViewModel = new PagingViewModel<Limit>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            return View(LimitsPagingViewModel);
        }
        
        [HttpGet]
        public IActionResult CreateLimit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateLimit(LimitInfoViewModel model)
        {
            
            validationService.ValidateLimitCreating(model, ModelState);

            if (ModelState.IsValid)
            {
                limitService.CreateLimit(model);

                return RedirectToAction("Index", "LimitPaymentCode");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult EditLimit(int limitId)
        {
            Limit limit = limitService.FindLimitId(limitId);
            EditLimitViewModel model = new EditLimitViewModel { Limit= limit, LimitInfo = new LimitInfoViewModel { LimitName = limit.LimitName, LimitAmount = limit.LimitAmount.ToString() } };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditLimit(EditLimitViewModel model)
        {
            validationService.ValidateLimitEditing(model, ModelState);
            if (ModelState.IsValid)
            {
                limitService.EditLimit(model.Limit.Id, model.LimitInfo);
                return RedirectToAction("Index", "LimitPaymentCode");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> PaymentCodeList(int page = 1)
        {
            IQueryable<PaymentСode> paymentCodies = paymentCodeService.GetPaymentСodeList().OrderBy(c => c.Code);
            PagedObject<PaymentСode> pagedObject = await pagingService.DoPage<PaymentСode>(paymentCodies, page);

            PagingViewModel<PaymentСode> PaymentCodiesPagingViewModel = new PagingViewModel<PaymentСode>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            return View(PaymentCodiesPagingViewModel);
        }

        [HttpGet]
        public IActionResult CreatePaymentCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePaymentCode(PaymentCodeInfoViewModel model)
        {
            PaymentСode paymentСode = paymentCodeService.FindPaymentСodeIsCode(model.Code);
            
            if (paymentСode != null)
            {
                ModelState.AddModelError("Code", "Такой код платеж уже существует");
            }
            
            if (ModelState.IsValid)
            {
                paymentCodeService.CreatePaymentСode(model);

                return RedirectToAction("PaymentCodeList", "LimitPaymentCode");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult EditPaymentCode(int paymentCodeId)
        {
            PaymentСode paymentСode = paymentCodeService.FindPaymentСodeId(paymentCodeId);
            EditPaymentCodeViewModel model = new EditPaymentCodeViewModel { PaymentCode = paymentСode, PaymentCodeInfo = new PaymentCodeInfoViewModel { Code = paymentСode.Code, PaymentCodeName = paymentСode.PaymentCodeName } };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPaymentCode(EditPaymentCodeViewModel model)
        {
            PaymentСode paymentСode = paymentCodeService.FindPaymentСodeIsCode(model.PaymentCodeInfo.Code);
            if (paymentСode != null && paymentСode.Id != model.PaymentCode.Id)
            {
                ModelState.AddModelError("PaymentCodeInfo.Code", "Такой код платеж уже существует");
            }

            if (ModelState.IsValid)
            {
                paymentCodeService.EditPaymentСode(model.PaymentCode.Id, model.PaymentCodeInfo);
                return RedirectToAction("PaymentCodeList", "LimitPaymentCode");
            }
            else
            {
                return View(model);
            }
        }
    }
}
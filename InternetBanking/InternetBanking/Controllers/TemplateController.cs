using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _templateServiceService;
        private readonly IUserService userService;
        private readonly ISelectListService selectListService;
        private readonly IValidationService validationService;
        private readonly IAccountService accountService;
        private readonly ITransferService transferService;
        private readonly IPaymentScheduleService paymentScheduleService;

        public TemplateController(ITemplateService templateServiceService, IUserService userService,
            ISelectListService selectListService, IValidationService validationService,
            IAccountService accountService, ITransferService transferService, IPaymentScheduleService paymentScheduleService)
        {
            this._templateServiceService = templateServiceService;
            this.userService = userService;
            this.selectListService = selectListService;
            this.validationService = validationService;
            this.accountService = accountService;
            this.transferService = transferService;
            this.paymentScheduleService = paymentScheduleService;
        }


        public async Task<IActionResult> Index()
        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            List<TemplateScheduleViewModel> templates = await _templateServiceService.GetTempaleList(user).OrderBy(t => t.TempalteName).Select(t=> new TemplateScheduleViewModel
            {
                Template = t
            }).ToListAsync();
            foreach (var t in templates)
            {
                t.IsScheduleExist = await paymentScheduleService.IsPaymentScheduleExist(t.Template);
            }
            return View(templates);
        }

        [HttpGet]
        public IActionResult TemplateSave(int tempalteId)
        {
            TemplateViewModel model = new TemplateViewModel
            {
                TemplateId = tempalteId,
                PaymentScheduleViewModel = new PaymentScheduleViewModel
                {
                    IntervalTypes = selectListService.GetIntervalTypes(),
                    DateStart = DateTime.Now.AddDays(1)
                }
            };
          

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TemplateSave(TemplateViewModel model)
        {
            validationService.ValidatePaymentSchedule(model, ModelState);
            if (ModelState.IsValid)
            {
                Template template = _templateServiceService.FindTemplateById(model.TemplateId);
                _templateServiceService.AddTemplateNameDisc(template, model);
                if (model.IsSetSchedule)
                {
                    IntervalType interval = await paymentScheduleService.FindIntervalTypeByIntervalCode(model.PaymentScheduleViewModel.IntervalCode.Value);
                    paymentScheduleService.CreatePaymentSchedule(template, interval,
                        model.PaymentScheduleViewModel.DateStart, model.PaymentScheduleViewModel.DateEnd);
                }
                return RedirectToAction("Transfer", "Transfer");
            }
            model.PaymentScheduleViewModel.IntervalTypes = selectListService.GetIntervalTypes();
            return View(model);
        }

        [HttpGet]
        public IActionResult TemplateTransfer(int templateId)
        {
            Template template = _templateServiceService.FindTemplateById(templateId);
            if (template.Type.Name == "InnerTransfer")
            {
                return RedirectToAction("InnerTransferTemplate", "Transfer" , new { templateId = template.Id});
            }

            if (template.Type.Name == "InterBankTransfer")
            {

                return RedirectToAction("InterTransferTemplate", "Transfer", new { templateId = template.Id });
            }
        

            return RedirectToAction("Transfer", "Transfer");
        }

        public IActionResult CreateTemplateInnerTransfer()
        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            CreateTemplateInnerTransferViewModel model = new CreateTemplateInnerTransferViewModel();
            InnerTransferViewModel transferModel = new InnerTransferViewModel();
            model.Transfer = transferService.GetMethodInnerTransfer(user, transferModel);
            model.Template = new TemplateViewModel
            {
                PaymentScheduleViewModel = new PaymentScheduleViewModel
                {
                    IntervalTypes = selectListService.GetIntervalTypes(),
                    DateStart = DateTime.Now.AddDays(1)
                }
            };
          
            return View(model);
        }

        [HttpPost]
        public  async Task<IActionResult> CreateTemplateInnerTransfer(CreateTemplateInnerTransferViewModel model)
        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            await validationService.ValidateInnerTransferTemplate(model.Transfer, user, ModelState);
            validationService.ValidatePaymentSchedule(model.Template, ModelState);
            if (ModelState.IsValid)
            {
                decimal amount = decimal.Parse(model.Transfer.Amount);
                Account sender = await accountService.FindAccountById(model.Transfer.AccountSenderId.Value);
                Account receiver = await accountService.FindAccountByNumber(model.Transfer.ReceiverAccountNumber);
                Template template = _templateServiceService.CreateTemplateInnerTransfer(sender, receiver, amount, model.Transfer.Comment, user);
                _templateServiceService.AddTemplateNameDisc(template, model.Template);
                if (model.Template.IsSetSchedule)
                {
                    IntervalType interval = await paymentScheduleService.FindIntervalTypeByIntervalCode(model.Template.PaymentScheduleViewModel.IntervalCode.Value);
                    paymentScheduleService.CreatePaymentSchedule(template, interval,
                        model.Template.PaymentScheduleViewModel.DateStart,
                        model.Template.PaymentScheduleViewModel.DateEnd);
                }
                
                return RedirectToAction("Index", "Template");
            }

            model.Transfer = transferService.GetMethodInnerTransfer(user, model.Transfer);
            model.Template.PaymentScheduleViewModel.IntervalTypes = selectListService.GetIntervalTypes();
            return View(model);

        }

        [HttpGet]
        public IActionResult CreateTemplateInterBankTransfer()
        {
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            InterBankTransferViewModel model = new InterBankTransferViewModel { Banks = selectListService.GetBankList(), PaymentCode = selectListService.GetPayemntCodeList(),
                Transfer = new InnerTransferViewModel() };
            model.Transfer = transferService.GetMethodInnerTransfer(user, model.Transfer);
            CreateTemplateInterBankTransferViewModel modelTemplate =
                new CreateTemplateInterBankTransferViewModel
                {
                    Transfer = model,
                    Template = new TemplateViewModel
                    {
                        PaymentScheduleViewModel = new PaymentScheduleViewModel
                        {
                            IntervalTypes = selectListService.GetIntervalTypes(),
                            DateStart = DateTime.Now.AddDays(1)
                        }
                    }
                };
         
            return View(modelTemplate);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplateInterBankTransfer(CreateTemplateInterBankTransferViewModel model)
        {
            Account receiver = accountService.OurBankAccount();
            User user = userService.FindUserByName(HttpContext.User.Identity.Name);
            await validationService.ValidateInterTransfer(model.Transfer.Transfer, user, receiver, ModelState);
            validationService.ValidatePaymentSchedule(model.Template, ModelState);
            if (ModelState.IsValid)
            {
                decimal amount = decimal.Parse(model.Transfer.Transfer.Amount);
                Account sender = await accountService.FindAccountById(model.Transfer.Transfer.AccountSenderId.Value);
                Template template = _templateServiceService.CreateTemplateInnerTransfer(sender, receiver, amount, model.Transfer.Transfer.Comment, user);
                _templateServiceService.AddTemplateNameDisc(template, model.Template);
                _templateServiceService.CreateTemplateInterTransfer(template, model.Transfer);
                if (model.Template.IsSetSchedule)
                {
                    IntervalType interval = await paymentScheduleService.FindIntervalTypeByIntervalCode(model.Template.PaymentScheduleViewModel.IntervalCode.Value);
                    paymentScheduleService.CreatePaymentSchedule(template, interval,
                        model.Template.PaymentScheduleViewModel.DateStart,
                        model.Template.PaymentScheduleViewModel.DateEnd);
                }
                return RedirectToAction("Index","Template");
            }
            model.Transfer.Banks = selectListService.GetBankList();
            model.Transfer.PaymentCode = selectListService.GetPayemntCodeList();
            model.Transfer.Transfer = transferService.GetMethodInnerTransfer(user, model.Transfer.Transfer);
            model.Template.PaymentScheduleViewModel.IntervalTypes = selectListService.GetIntervalTypes();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddPaymentSchedule(int templateId)
        {
            Template template =  _templateServiceService.FindTemplateById(templateId);
            PaymentScheduleViewModel model = new PaymentScheduleViewModel
            {
                DateStart = DateTime.Now.AddDays(1),
                TemplateName = template.TempalteName,
                IntervalTypes = selectListService.GetIntervalTypes(),
                TemplateId = template.Id
            };
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentSchedule(PaymentScheduleViewModel model)
        {
             validationService.ValidateAddPaymentSchedule(model, ModelState);
            Template template = _templateServiceService.FindTemplateById(model.TemplateId);
            if (ModelState.IsValid)
            {
              
                IntervalType interval =
                    await paymentScheduleService.FindIntervalTypeByIntervalCode(model.IntervalCode.Value);
                paymentScheduleService.CreatePaymentSchedule(template, interval,
                    model.DateStart,
                    model.DateEnd);
                return RedirectToAction("Index", "Template");
            }
            model.IntervalTypes = selectListService.GetIntervalTypes();
            model.TemplateName = template.TempalteName;
            return View(model);
        }


       
       

        [HttpPost]
        public async Task<IActionResult> DeletePaymentSchedule(int templateId)
        {
            Template template = _templateServiceService.FindTemplateById(templateId);
            PaymentSchedule schedule = await paymentScheduleService.FindPaymentSheduleByTemplate(template);
            
            if (schedule != null)
            {
                User user =  userService.FindUserByName(HttpContext.User.Identity.Name);
                Account account = await accountService.FindAccountById(schedule.Template.AccountSenderId.Value);

                if (await accountService.IsUserHaveRightsOnAccount(user, schedule.Template.AccountSenderId.Value))
                {
                    paymentScheduleService.DeletePaymentSchedule(schedule.Id);
                    return Json(new { state = true, message = "Расписание удалено." });
                }
                return Json(new{state = false, message ="Ошибка доступа!"});
            }
            return Json(new {state = false, message = "Такого расписание нет!"});
        }
    } 
}
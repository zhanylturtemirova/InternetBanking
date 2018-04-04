using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Controllers
{
    public class BankController : Controller
    {
        private ApplicationContext context;
        private IHomePagingService pagingService;
        private readonly IBankService bankService;
        private readonly IValidationService validationService;

        public BankController(ApplicationContext context, IHomePagingService pagingService, IBankService bankService, IValidationService validationService)
        {
            this.context = context;
            this.pagingService = pagingService;
            this.bankService = bankService;
            this.validationService = validationService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Bank> banks = bankService.GetBankList().OrderBy(b => b.BIK); 
            PagedObject<Bank> pagedObject = await pagingService.DoPage<Bank>(banks, page);

            PagingViewModel<Bank> BanksPagingViewModel = new PagingViewModel<Bank>
            {
                PageViewModel = new PageViewModel(pagedObject.Count, page, pagedObject.PageSize),
                Objects = pagedObject.Objects
            };

            return View(BanksPagingViewModel);
        }

        public IActionResult BankInfo(int bankId)
        {
            Bank bank = bankService.GetBank(bankId);
            return View(bank);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBankViewModel model)
        {
            validationService.ValidateBankCreating(model, ModelState);
            if (ModelState.IsValid)
            {
                BankInfo bankInfo = bankService.CreateBankInfo(model);
                bankService.CreateBank(model, bankInfo);

                return RedirectToAction("Index", "Bank");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Edit(int bankId)
        {
            Bank bank = bankService.GetBank(bankId);
            EditBankViewModel model = new EditBankViewModel { Bank = bank, BankInfo = new CreateBankViewModel { BIK = bank.BIK, Email= bank.BankInfo.Email, BankName= bank.BankInfo.BankName } };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditBankViewModel model)
        {
            validationService.ValidateBankEditing(model, ModelState);

            if (ModelState.IsValid)
            {
                bankService.EditBankInfo( model.Bank.BankInfo, model.BankInfo);
                bankService.EditBank(model.Bank.Id, model.BankInfo);
                return RedirectToAction("Index", "Bank");
            }
            else
            {
                return View(model);
            }
        }
    }
}
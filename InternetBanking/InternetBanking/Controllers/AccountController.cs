using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetBanking.ViewModels.Paging;

namespace InternetBanking.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ISelectListService selectListService;
        private readonly ApplicationContext context;
        private readonly UserManager<User> userManager;
        private readonly IHomePagingService pagingService;
        private readonly ICompanyService companyService;
        private readonly IUserService userService;
        private readonly IEmployeeService employeeService;

        public AccountController(IAccountService accountService, ISelectListService selectListService, ApplicationContext context, UserManager<User> userManager, IHomePagingService pagingService, ICompanyService companyService, IUserService userService, IEmployeeService employeeService)
        {
            this.accountService = accountService;
            this.selectListService = selectListService;
            this.context = context;
            this.userManager = userManager;
            this.pagingService = pagingService;
            this.companyService = companyService;
            this.userService = userService;
            this.employeeService = employeeService;
        }

        public IActionResult UserAccountCreate(string userId)
        {
            CreateUserAccountViewModel model = new CreateUserAccountViewModel
            {
               Limits = selectListService.GetLimits(),
               Currencies = selectListService.GetCurrencies(),
               UserId = userId
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserAccountCreate(CreateUserAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.UserId);
                Currency currency = await context.Currencies.FirstOrDefaultAsync(c => c.Id == model.CurrencyId);
                Account account = accountService.CreateAccount(user, currency);
                accountService.AddAccount(account);
                userService.UserAccountLimit(account,model.UserId,model.LimitId);
                return RedirectToAction("UserAccounts", "Account", new { userId = model.UserId });
            }
            model.Limits = selectListService.GetLimits();
            model.Currencies = selectListService.GetCurrencies();
            return View(model);
        }

        public IActionResult CompanyAccountCreate(int companyId)
        {

            
            List<EmployeeAccountViewModel> employeesAccounts = employeeService.GetEmployeesByCompanyId(companyId)

                .Select(e=>new EmployeeAccountViewModel{Employee = e, EmployeeId = e.Id,  Limits = selectListService.GetLimits(), }).ToList();
            
            CompanyAccountCreateViewModel model = new CompanyAccountCreateViewModel
            {
                Currencies = selectListService.GetCurrencies(),
                CompanyId = companyId,
                EmployeeAccounts = employeesAccounts
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CompanyAccountCreate(CompanyAccountCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Company company = await context.Companies.FirstOrDefaultAsync(c => c.Id == model.CompanyId);
                Currency currency = await context.Currencies.FirstOrDefaultAsync(c => c.Id == model.CurrencyId);
                Account account = accountService.CreateAccount(company, currency);
                accountService.AddAccount(account);

                foreach (var acc in model.EmployeeAccounts)
                {
                    acc.Account = account;
                }
                companyService.AddRangeEmployeeAccounts(model.EmployeeAccounts);

                return RedirectToAction("CompanyInfo", "Company", new {id = company.Id});

            }

            foreach (EmployeeAccountViewModel limit in model.EmployeeAccounts)
            {
                limit.Limits = selectListService.GetLimits();
            }

            model.Currencies = selectListService.GetCurrencies();
            var limits = selectListService.GetLimits();
            foreach (var employeeAccountViewModel in model.EmployeeAccounts)
            {
                employeeAccountViewModel.Employee =
                    employeeService.FindEmployeeById(employeeAccountViewModel.EmployeeId.Value);
                employeeAccountViewModel.Limits = limits;
            }
            return View(model);

        }

        public async Task<ActionResult> Index(int companyId, int page = 1)
        {
            List<AccountWithBalance> accounts = accountService.GetCompanyAccounts(companyId);
            
            ViewBag.OwnerId = companyId;
            ViewBag.OwnerName = companyService.FindCompanyById(companyId).NameCompany;

            return View(accounts);
        }

        public async Task<ActionResult> UserAccounts(string userId, int page = 1)
        {
            UserInfo user = await userService.FindUserInfoByUserId(userId);
            List<AccountWithBalance> accounts = accountService.GetUserInfoAccounts(user.Id);
            UserAccountViewModel userAccounts = new UserAccountViewModel { UserInfo = user, Accounts = accounts };
            return View(userAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> AccountInfo(int id)
        {
            Account currentAccount = await accountService.FindAccountById(id);
            AccountWithBalance model = new AccountWithBalance()
            {
                Account = currentAccount,
                Balance = accountService.GetAccountBalance(currentAccount).Result
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(int accountId)
        {
            Account account = await accountService.FindAccountById(accountId);
            EditAccountViewModel model = await accountService.GetEditAccountViewModel(account);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                accountService.UpdateAccount(model);
                return RedirectToAction("AccountInfo", "Account", new {id = model.AccountId});
            }
            var limits = selectListService.GetLimits();
           Account account = await accountService.FindAccountById(model.AccountId);
            ;

            foreach (var userAccount in model.UserAccounts)
            {
                userAccount.Account = account;
                userAccount.Limits = limits;
                if (userAccount.EmployeeId != null)
                {
                    userAccount.Employee = employeeService.FindEmployeeById(userAccount.EmployeeId.Value);
                }
                if (userAccount.UserInfoId != null)
                {
                    userAccount.UserInfo = await userService.FindUserInfoById(userAccount.UserInfoId.Value);
                }
                
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Block(int accountId)
        {
            Account account = await accountService.FindAccountById(accountId);
            if (account != null)
            {
                if (!account.Locked)
                {
                    accountService.BlockAccount(accountId);
                    return Json(new {state = true, message = "Счет заблокирован"});
                }
                return Json(new {state = false, message = "Счет уже был заблокирован!"});
            }
            return Json(new { state = false, message = "Такого счета нет!" });


        }
        [HttpPost]
        public async Task<IActionResult> Unblock(int accountId)
        {
            Account account = await accountService.FindAccountById(accountId);
            if (account != null)
            {
                if (account.Locked)
                {
                    accountService.UnblockAccount(accountId);
                    return Json(new { state = true, message = "Счет разблокирован" });
                }
                return Json(new { state = false, message = "Счет уже был разблокирован!" });
            }
            return Json(new { state = false, message = "Такого счета нет!" });



        }
    }
}
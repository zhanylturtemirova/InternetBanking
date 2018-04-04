using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Remotion.Linq.Clauses;

namespace InternetBanking.Controllers
{
    public class AdminController : Controller
    {
        private readonly IStringLocalizer<AdminController> localizer;
        private readonly ISelectListService selectListService;
        private ApplicationContext context;
        private UserManager<User> userManager;
        private readonly ICurrencyService currencyService;
        private readonly IUserService userService;

        public AdminController(IStringLocalizer<AdminController> localizer, ISelectListService selectListService, ApplicationContext context, UserManager<User> userManager, ICurrencyService currencyService, IUserService userService)
        {
            this.localizer = localizer;
            this.selectListService = selectListService;
            this.context = context;
            this.userManager = userManager;
            this.currencyService = currencyService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            int clientsCount = context.Users.Count();
            int companiesCount = context.Companies.Count();
            int ordanaryUsersCount = context.UserInfo.Count();
            int employeesUsersCount = context.EmployeeInfos.Count();
            int blockedUsersCount = context.Users.Where(u=>u.IsBlocked==true).Count();
            string query = "exec sp_spaceused";
           // string query = "use \"InternetBankingdb\"\r\nexec sp_spaceused";
            var blogs = context.Database.ExecuteSqlCommand(query);
          
            ViewBag.ClientsCount = clientsCount;
            ViewBag.CompaniesCount = companiesCount;
            ViewBag.OrdanaryUsersCount = ordanaryUsersCount;
            ViewBag.EmployeesUsersCount = employeesUsersCount;
            ViewBag.BlockedUsersCount = blockedUsersCount;
            ViewBag.DbSize = blogs.ToString();
            return View();
        }

        //public IActionResult  NativeCurrency()
        //{
        //    if (!User.IsInRole("admin"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

            
        //    return View();
        //}

        public async Task<IActionResult> PasswordBlackList()
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<BlackList> blackLists = context.BlackListedPasswords.ToList();
            return View(blackLists);
        }

       

        [Authorize(Roles = "admin")]
        public IActionResult CreateBlackList()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBlackList(BlackListPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                BlackList blackListedPassword = new BlackList()
                {
                    Id = model.Id,
                    BlackListedPassword = model.Name
                };
                context.BlackListedPasswords.Add(blackListedPassword);
                context.SaveChanges();
                return RedirectToAction("PasswordBlackList");
            }
            else
            {
                ModelState.AddModelError(string.Empty, localizer["Input"]);
            }

            return View(model);
        }


        [Authorize(Roles = "admin")]
        public IActionResult EditBlackList(int id)
        {
            BlackList blackList = context.BlackListedPasswords.Find(id);

            BlackListPasswordViewModel model = new BlackListPasswordViewModel
            {

                Id = blackList.Id,
                Name = blackList.BlackListedPassword
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public IActionResult EditBlackList(BlackListPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                BlackList blackList = new BlackList()
                {
                    Id = model.Id,
                    BlackListedPassword = model.Name
                };

                context.BlackListedPasswords.Update(blackList);
                context.SaveChanges();
                return RedirectToAction("PasswordBlackList");
            }
            else
            {
                ModelState.AddModelError(string.Empty, localizer["Input"]);
            }

            return View(model);
        }
        [Authorize(Roles = "admin")]
        public IActionResult DeleteBlackLisedPassword(int id)
        {
            BlackList blackList = context.BlackListedPasswords.Find(id);
            context.BlackListedPasswords.Remove(blackList);
            context.SaveChanges();

            return RedirectToAction("PasswordBlackList");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BlockUser(string  userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var result = await userService.BlockUser(user);
            if (result.Succeeded)
            {
                return Json(new
                {
                    state = true,
                    message = localizer["UserLocked"],
                    errors = result.Errors
                });
            }
            else
            {
                return Json(new
                {
                    state = false,
                    message = localizer["UserNotLocked"],
                    errors = result.Errors
                });
            }
            
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnBlockUser(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var result = await userService.UnblockUser(user);
            if (result.Succeeded)
            {
             
                return Json(new
                {
                    state = true,
                    message = localizer["UserUnLocked"],
                    errors = result.Errors
                });
            }
            else
            {
                return Json(new
                {
                    state = false,
                    message = localizer["UserNotUnLocked"],
                    errors = result.Errors
                });
            }
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChooseNativeCurrency()
        {
           
            Currency nativeCurrency = await context.Currencies.FirstOrDefaultAsync(c => c.IsNativeCurrency);
            NativeCurrencyViewModel model = new NativeCurrencyViewModel()
            {
                
                NativeCurrencyName = nativeCurrency.Name
                
            };

            model.Currencies = selectListService.GetCurrencies();
          
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> ChooseNativeCurrency(NativeCurrencyViewModel model)
        {
            Currency chosenCurrency = await context.Currencies.FirstOrDefaultAsync(c => c.Id == model.CurrencyId);
            if (chosenCurrency == null)
            {
                ModelState.AddModelError("CurrencyId", localizer["ChosenCurrencyNull"]);
            }
            if (ModelState.IsValid)
            {
                List<Currency> currencies = context.Currencies.Where(c => c.IsNativeCurrency).ToList();
                foreach (var c in currencies)
                {
                    c.IsNativeCurrency = false;
                    currencyService.UpdateCurrency(c);
                }
                
                chosenCurrency.IsNativeCurrency = true;
               currencyService.UpdateCurrency(chosenCurrency);
               context.SaveChanges();
              return RedirectToAction("Index", "Admin");
            }
        
            NativeCurrencyViewModel newModel = new NativeCurrencyViewModel()
            {
                NativeCurrencyName = chosenCurrency.Name
            };
            newModel.Currencies = selectListService.GetCurrencies();
            return View(model);
        }

    }
}








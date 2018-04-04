using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;

namespace InternetBanking.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        [Authorize]
        public IActionResult Index()
        {
            if(!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Currency> currencies = currencyService.GetCurrencies();
            return View(currencies);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CurrencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (currencyService.IsUniqueCode(model.Code))
                {
                    Currency currency = new Currency()
                    {
                        Id = model.Id,
                        Code = model.Code,
                        Name = model.Name,
                        IsNativeCurrency = false,
                    };
                    currencyService.AddCurrency(currency);

                    return RedirectToAction("Index", "Currency");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Не уникальный код");
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            Currency currency = currencyService.FindCurrencyById(id);

            CurrencyViewModel model = new CurrencyViewModel
            {
                Code = currency.Code,
                Id = currency.Id,
                Name = currency.Name,
                IsNativeCurrency = false
             
                
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CurrencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (currencyService.IsUniqueCode(model.Code) )
                {
                    Currency currency = new Currency()
                    {
                        Id = model.Id,
                        Code = model.Code,
                        Name = model.Name,
                        IsNativeCurrency = false
                        
                    };
                    currencyService.UpdateCurrency(currency);

                    return RedirectToAction("Index", "Currency");
                }
                else  
                {

                    ModelState.AddModelError(string.Empty, "Не уникальный код");
                }
            }
            return View(model);
        }

        //[Authorize(Roles = "admin")]
        //public IActionResult Delete(int id)
        //{
        //    Currency currency = currencyService.FindCurrencyById(id);
        //    currencyService.RemoveCurrency(currency);

        //    return RedirectToAction("Index", "Currency");
        //}


        



      












    }
}
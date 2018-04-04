using InternetBanking.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using InternetBanking.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace InternetBanking.Tests
{
    public class CurrencyServiceTests
    {
        public ApplicationContext context;
        public ICurrencyService currencyService;
        public CurrencyServiceTests()
        {
            context = TestServicesProvider.GetContext();
            currencyService = new CurrencyService(context);
            TestServicesProvider.GetModelTestData().FillData();
        }
        
        [Fact]
        public void FindCurrencyById()
        {
            bool currencyIsExist = currencyService.IsExist(GetLastCurrency().Id);
            Assert.True(currencyIsExist);
        }

        [Fact]
        public void AddCurrency()
        {
            Currency currency = new Currency()
            {
                Code = "679",
                Name = "Тенге"
            };

            currencyService.AddCurrency(currency);
            bool currencyIsExist = currencyService.IsExist(currency.Id);
            Assert.True(currencyIsExist);
        }

        [Fact]
        public void GetCurrencies()
        {
            List<Currency> currencyList = currencyService.GetCurrencies();
            Assert.Equal(currencyList, context.Currencies.ToList());
        }

        

        [Fact]
        public void UpdateCurrency()
        {
            Currency currency = GetLastCurrency();
            currency.Name = "won";
            currencyService.UpdateCurrency(currency);
            Assert.Equal("won", currency.Name);
        }

        public Currency GetLastCurrency()
        {
            return context.Currencies.Last();
        }
    }
}

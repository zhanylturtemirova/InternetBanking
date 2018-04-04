using InternetBanking.Models;
using InternetBanking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternetBanking.ViewModels.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InternetBanking.Tests
{
    public class ExchangeRateServiceTests
    {
        private readonly ApplicationContext context ;
        private readonly IExchangeRateService exchangeRateService;
        private IAccountService accountService;
        public ExchangeRateServiceTests()
        {
            TestServicesProvider.GetModelTestData().FillData();
            context = TestServicesProvider.GetContext();
            exchangeRateService = TestServicesProvider.GetExchangeRateService();
            accountService = TestServicesProvider.GetAccountService();
        }

        [Fact]
        public void AddExchangeRate()
        {
           /* List<Currency> currencies = context.Currencies.Where(c => c.Name == "USD").ToList();
            for (int i = 0; i < currencies.Count; i++)
            {
                context.Entry(currencies[i]).State = EntityState.Deleted;
            }
            context.SaveChanges();*/
             Currency currency = new Currency
             {
                 Code = "222",
                 Name = "USD",
                 IsNativeCurrency = false
 
             };
             context.Currencies.Add(currency);
             context.SaveChanges();
             ExchangeRate exchangeRate = new ExchangeRate
             {
                 ExchangeRateTypeId = context.ExchangeRateTypes.FirstOrDefault(ert=>ert.IsEqual(ExchangeRateTypesEnum.Market)).Id,
                 CurrencyId = currency.Id,
                 RateForSale = 75,
                 RateForPurchaise = 76,
                 RateDate = DateTime.Now
             };
 
             exchangeRateService.AddExchangeRate(exchangeRate);
             bool rateIsExist = exchangeRateService.IsExist(exchangeRate.Id);
             Assert.True(rateIsExist);
 
             context.Entry(exchangeRate).State = EntityState.Deleted;
             context.Entry(currency).State = EntityState.Deleted;
             context.SaveChanges();
             
        }

        [Fact]
        public void FindExchangeRateById()
        {
            Currency currency = new Currency
            {
                Code = "222",
                Name = "USD",
                IsNativeCurrency = false

            };
            context.Currencies.Add(currency);
            context.SaveChanges();
            ExchangeRate exchangeRate = new ExchangeRate
            {
                ExchangeRateTypeId = context.ExchangeRateTypes.FirstOrDefault(ert => ert.IsEqual(ExchangeRateTypesEnum.Market)).Id,
                CurrencyId = currency.Id,
                RateForSale = 75,
                RateForPurchaise = 76,
                RateDate = DateTime.Now
            };

            exchangeRateService.AddExchangeRate(exchangeRate);
            bool currencyIsExist = exchangeRateService.IsExist(GetLastExchangeRate().Id);
            Assert.True(currencyIsExist);
            context.Entry(currency).State = EntityState.Deleted;
            context.SaveChanges();

        }

        [Fact]
        public void GetExchangeRates()
        {
            List<ExchangeRate> exchangeList = exchangeRateService.GetExchangeRates();
            var result = context.ExchangeRates.ToList();
            Assert.Equal(exchangeList.Count, result.Count);
            for (int i = 0; i < exchangeList.Count; i++)
            {
                Assert.Equal(exchangeList[i].Id, result[i].Id);
                Assert.Equal(exchangeList[i].RateDate, result[i].RateDate);
                Assert.Equal(exchangeList[i].RateForPurchaise, result[i].RateForPurchaise);
                Assert.Equal(exchangeList[i].RateForSale, result[i].RateForSale);
                Assert.Equal(exchangeList[i].ExchangeRateTypeId, result[i].ExchangeRateTypeId);
                Assert.Equal(exchangeList[i].CurrencyId, result[i].CurrencyId);
            }
        }

        [Fact]
        public void GetLastExchangeRatesByDate()
        {
            IQueryable<ExchangeRate> exchangeList = exchangeRateService.GetLastExchangeRatesByDate();
            bool IsInList = exchangeList.Any(r => r.RateDate == GetLastExchangeRate().RateDate);
            Assert.True(IsInList);
        }

        [Fact]
        public void RemoveExchangeRate()
        {
            ExchangeRate exchangeRate = GetLastExchangeRate();
            int ExchangeRateId = exchangeRate.Id;
            exchangeRateService.RemoveExchangeRate(exchangeRate);
            bool rateIsExist = exchangeRateService.IsExist(ExchangeRateId);
            Assert.False(rateIsExist);
        }

        [Fact]
        public void UpdateExchangeRate()
        {
            ExchangeRate exchangeRate = GetLastExchangeRate();
            exchangeRate.RateForPurchaise = 65;
          
            exchangeRateService.UpdateExchangeRate(exchangeRate);
            Assert.Equal(65, exchangeRate.RateForPurchaise);
        }

        public ExchangeRate GetLastExchangeRate()
        {
            return context.ExchangeRates.OrderByDescending(r => r.Id).First();
        }
        
       
    }
}

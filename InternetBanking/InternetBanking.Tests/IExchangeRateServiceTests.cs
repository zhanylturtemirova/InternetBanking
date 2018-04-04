using InternetBanking.Models;
using InternetBanking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Xunit;

namespace InternetBanking.Tests
{
    public class IExchangeRateServiceTests
    {
        public ApplicationContext context;
        public IExchangeRateService exchangeRateService;
        private IAccountService accountService;
        public IExchangeRateServiceTests()
        {
            context = TestServicesProvider.GetContext();
            exchangeRateService = new ExchangeRateService(context, accountService);
        }

        [Fact]
        public void AddExchangeRate()
        {
            ExchangeRate exchangeRate = new ExchangeRate()
            {
                CurrencyId = 2,
                RateForSale = 75,
                RateForPurchaise = 76,
                RateDate = DateTime.Now
            };

            exchangeRateService.AddExchangeRate(exchangeRate);
            bool rateIsExist = exchangeRateService.IsExist(exchangeRate.Id);
            Assert.True(rateIsExist);
        }

        [Fact]
        public void FindExchangeRateById()
        {
            bool currencyIsExist = exchangeRateService.IsExist(GetLastExchangeRate().Id);
            Assert.True(currencyIsExist);
        }

        [Fact]
        public void GetExchangeRates()
        {
            List<ExchangeRate> exchangeList = exchangeRateService.GetExchangeRates();
            Assert.Equal(exchangeList, context.ExchangeRates.ToList());
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

        [Fact]
        public void CurrencyConverter()
        {
           // string ammountSend = "1000";
            int ammountSend = 1000;
           Currency currencyFrom = new Currency()
            {
              
                Name= "KZT",
                Code="123",
                IsNativeCurrency = false,
            };
            Currency currencyTo = new Currency()
            {
               
                Name = "USD",
                Code = "456",
                IsNativeCurrency = false,
            };
            var  x =context.Currencies.Add(currencyTo);
            var y =context.Currencies.Add(currencyFrom);
            var res = context.SaveChanges();

            ExchangeRate exchangeRate1 = new ExchangeRate()
            {
                RateForSale = Convert.ToDecimal(0.22),
                RateForPurchaise = Convert.ToDecimal(0.23),
                RateDate=DateTime.Now,
                CurrencyId= currencyFrom.Id,
                ExchangeRateTypeId=2
            };
            ExchangeRate exchangeRate2 = new ExchangeRate()
            {
                RateForSale = Convert.ToDecimal(68),
                RateForPurchaise = Convert.ToDecimal(69),
                RateDate = DateTime.Now,
                CurrencyId = currencyTo.Id,
                ExchangeRateTypeId = 2
            };
            exchangeRateService.AddExchangeRate(exchangeRate1);
            exchangeRateService.AddExchangeRate(exchangeRate2);
            //context.ExchangeRates.Add(exchangeRate1);
            //context.ExchangeRates.Add(exchangeRate2);
            //context.SaveChanges();
            Account account1 = new Account()
            {
                Number = 123456789123.ToString(),
                Locked = false,
                CurrencyId = currencyFrom.Id

            };
            Account account2= new Account()
            {
                Number = 123456712345.ToString(),
                Locked = false,
                CurrencyId = currencyTo.Id,
             
            };
            var a = context.Accounts.Add(account1);
            var b = context.Accounts.Add(account2);

            decimal ammountSendInNativeCurrency;
            decimal ammountReceive;

            //var ammountReceive = exchangeRateService.CurrencyConverter(ammountSend.ToString(), account1.Id, account2.Id);
            if (!currencyFrom.IsNativeCurrency)
            {
                ExchangeRate convertToNativeCurrencyRate = context.ExchangeRates.FirstOrDefault(u => u.CurrencyId == currencyFrom.Id);

                ammountSendInNativeCurrency = Math.Round(ammountSend * convertToNativeCurrencyRate.RateForSale, 2);
            }
            else
            {
                ammountSendInNativeCurrency = ammountSend;
            }
            if (!currencyTo.IsNativeCurrency)
            {
                ExchangeRate convertToWantedCurrencyRate = context.ExchangeRates.FirstOrDefault(u => u.CurrencyId == currencyTo.Id);

                ammountReceive = Math.Round(ammountSendInNativeCurrency / convertToWantedCurrencyRate.RateForPurchaise, 2);
            }

            else
            {
                ammountReceive = ammountSendInNativeCurrency;
            }
            Assert.Equal(Convert.ToDecimal(3.19), ammountReceive);
            exchangeRateService.RemoveExchangeRate(exchangeRate1);
            exchangeRateService.RemoveExchangeRate(exchangeRate2);
 
          
            context.Entry(currencyTo).State = EntityState.Deleted;
            context.Entry(currencyFrom).State = EntityState.Deleted;
           
            context.Entry(account1).State = EntityState.Deleted;
            context.Entry(account2).State = EntityState.Deleted;
         
            context.SaveChanges();
        }

    }
}

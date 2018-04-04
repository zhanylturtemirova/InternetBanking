using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ApplicationContext context;
        private readonly IAccountService accountService;
     

        public ExchangeRateService(ApplicationContext context, IAccountService accountService)
        {
            this.context = context;
            this.accountService = accountService;
          
        }

        public void AddExchangeRate(ExchangeRate exchangeRate)
        {
             context.ExchangeRates.Add(exchangeRate);
             context.SaveChanges();
        }

        public ExchangeRate FindExchangeRateById(int id)
        {
            ExchangeRate exchangeRate = context.ExchangeRates.FirstOrDefault(r => r.Id == id);
            return exchangeRate;
        }

        public List<ExchangeRate> GetExchangeRates()
        {
            return context.ExchangeRates.ToList();
        }

        public IQueryable<ExchangeRate> GetLastExchangeRatesByDate()
        {
            var exchangeRatesMaxDate = from rateTable in context.ExchangeRates
                                       group rateTable by new { rateTable.CurrencyId, rateTable.ExchangeRateTypeId } into g
                                       select new { CurrencyId = g.Key.CurrencyId, ExchangeRateTypeId = g.Key.ExchangeRateTypeId,  RateDate = g.Max(ratedate => ratedate.RateDate)};

            IQueryable<ExchangeRate> exchangeRates = from rateTableMax in exchangeRatesMaxDate
                                                     join rateTable in context.ExchangeRates
                                                     on new { rateTableMax.CurrencyId, rateTableMax.RateDate, rateTableMax.ExchangeRateTypeId } equals
                                                        new { rateTable.CurrencyId, rateTable.RateDate, rateTable.ExchangeRateTypeId } into joined
                                                     from j in joined.DefaultIfEmpty()
                                                     select new ExchangeRate()
                                                     {
                                                         RateDate = rateTableMax.RateDate,
                                                         CurrencyId = j.CurrencyId,
                                                         RateForPurchaise = j.RateForPurchaise,
                                                         RateForSale = j.RateForSale,
                                                         Id = j.Id,
                                                         Currency = j.Currency,
                                                         ExchangeRateTypeId = j.ExchangeRateTypeId,
                                                         ExchangeRateType = j.ExchangeRateType
                                                     };

            return exchangeRates;
        }

        public void RemoveExchangeRate(ExchangeRate exchangeRate)
        {
            context.ExchangeRates.Remove(exchangeRate);
            context.SaveChanges();
        }

        public void UpdateExchangeRate(ExchangeRate exchangeRate)
        {
            context.ExchangeRates.Update(exchangeRate);
            context.SaveChanges();
        }

        public bool IsExist(int id)
        {
            return FindExchangeRateById(id) != null;
        }

        public List<ExchangeRateType> GetExchangeRateTypes()
        {
            return context.ExchangeRateTypes.ToList();
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

        public decimal CurrencyConverter(string ammountSend, int accountSenderId, int accountReceiverId)
        {
            decimal amountDecimal = 0;
            accountService.AmountTryParse(ammountSend, out amountDecimal);
            Account accountReceiver = accountService.FindAccountById(accountReceiverId).Result;
            Account accountSender = accountService.FindAccountById(accountSenderId).Result;
            List<ExchangeRate> exchangeRates = GetLastExchangeRatesByDate().ToList();
            decimal ammountSendInNativeCurrency = 0;
            decimal ammountReceive = 0;

            Currency currencyFrom = accountSender.Currency;
            Currency currencyTo = accountReceiver.Currency;
            if (!currencyFrom.IsNativeCurrency)
            {
                decimal convertToNativeCurrencyRate = exchangeRates.FirstOrDefault(u => u.CurrencyId == accountSender.CurrencyId).RateForSale;
                ammountSendInNativeCurrency = Math.Round(amountDecimal * convertToNativeCurrencyRate, 2);
            }
            else
            {
                ammountSendInNativeCurrency = amountDecimal;
            }
            if (!currencyTo.IsNativeCurrency)
            {
                decimal convertToWantedCurrencyRate = exchangeRates
                    .FirstOrDefault(u => u.CurrencyId == accountReceiver.CurrencyId).RateForPurchaise;
                ammountReceive = Math.Round(ammountSendInNativeCurrency / convertToWantedCurrencyRate, 2);
            }

            else
            {
                ammountReceive = ammountSendInNativeCurrency;
            }

            return ammountReceive;

        }

        public List<ExchangeRate> GetExchangeRates(int currencyId)
        {
            List<ExchangeRate> rates = context.ExchangeRates.ToList();
            return rates;
        }
    }
}

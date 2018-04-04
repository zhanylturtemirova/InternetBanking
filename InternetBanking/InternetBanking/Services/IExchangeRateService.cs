using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Services
{
    public interface IExchangeRateService
    {
        List<ExchangeRate> GetExchangeRates();
        IQueryable<ExchangeRate> GetLastExchangeRatesByDate();
        void AddExchangeRate(ExchangeRate exchangeRate);
        ExchangeRate FindExchangeRateById(int id);
        void UpdateExchangeRate(ExchangeRate exchangeRate);
        void RemoveExchangeRate(ExchangeRate exchangeRate);
        bool IsExist(int id);
        List<ExchangeRateType> GetExchangeRateTypes();
        bool RateTryParse(string modelAmount, out decimal amount);
        decimal CurrencyConverter(string ammountSend, int accountSenderId, int accountReceiverId);
    }
}

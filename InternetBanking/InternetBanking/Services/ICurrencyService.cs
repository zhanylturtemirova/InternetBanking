using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public interface ICurrencyService
    {
        List<Currency> GetCurrencies();
        void AddCurrency(Currency currency);
        Currency FindCurrencyById(int id);
        void UpdateCurrency(Currency currency);
       
        bool IsExist(int id);
        bool IsUniqueCode(string code);
        bool IsNativecurrencyExists();
    }
}

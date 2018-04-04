using InternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationContext context;

        public CurrencyService(ApplicationContext context)
        {
            this.context = context;
        }

        public void AddCurrency(Currency currency)
        {
            
            context.Currencies.Add(currency);
            context.SaveChanges();
        }

        public Currency FindCurrencyById(int id)
        {
            Currency currency = context.Currencies.FirstOrDefault(c => c.Id == id);
            return currency;
        }

        public List<Currency> GetCurrencies()
        {
            return context.Currencies.ToList();
        }

        

        public void UpdateCurrency(Currency currency)
        {
            context.Currencies.Update(currency);
            context.SaveChanges();
        }

        public bool IsExist(int id)
        {
            return FindCurrencyById(id) != null;
        }

        public bool IsUniqueCode(string code)
        {
            return !context.Currencies.Any(c => c.Code == code);
        }

        public bool IsNativecurrencyExists()
        {
            return context.Currencies.Any(u => u.IsNativeCurrency == true);

        }
    }
}

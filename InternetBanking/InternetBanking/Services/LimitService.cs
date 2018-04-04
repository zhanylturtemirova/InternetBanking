using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public class LimitService : ILimitService
    {
        private ApplicationContext context;

        public LimitService(ApplicationContext context)
        {
            this.context = context;
        }
        public Limit CreateLimit(LimitInfoViewModel model)
        {
            model.LimitAmount = model.LimitAmount.Replace('.', ',');
            Limit limit = new Limit
            {
                LimitName = model.LimitName,
                LimitAmount = Math.Round(Convert.ToDecimal(model.LimitAmount), 2)
        };

            context.Limits.Add(limit);
            context.SaveChanges();
            return limit;
        }
        public IQueryable<Limit> GetLimitList()
        {
            IQueryable<Limit> limits = context.Limits;
            return limits;
        }

        public Limit FindLimitId(int id)
        {
            Limit limit = GetLimitList().FirstOrDefault(l => l.Id == id);
            return limit;
        }
        public Limit FindLimitName(string limitName)
        {
            Limit limit = GetLimitList().FirstOrDefault(l => l.LimitName == limitName);
            return limit;
        }

        public Limit EditLimit(int limitId, LimitInfoViewModel model)
        {
            model.LimitAmount = model.LimitAmount.Replace('.', ',');
            Limit limit = FindLimitId(limitId);
            limit.LimitName = model.LimitName;
            limit.LimitAmount = Math.Round(Convert.ToDecimal(model.LimitAmount), 2);

            context.Limits.Update(limit);
            context.SaveChanges();
            return limit;
        }

        public bool AmountTryParse(string modelAmount, out decimal amount)
        {
            if (!string.IsNullOrEmpty(modelAmount))
            {
                modelAmount = modelAmount.Replace('.', ',');
            }
            bool tryParse = decimal.TryParse(modelAmount, out amount);
            amount = Math.Round(amount, 2);

            return tryParse;
        }
    }
}

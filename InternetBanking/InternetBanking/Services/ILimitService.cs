using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public interface ILimitService
    {
        Limit CreateLimit(LimitInfoViewModel model);
        IQueryable<Limit> GetLimitList();
        Limit FindLimitId(int id);
        Limit FindLimitName(string limitName);
        Limit EditLimit(int limitId, LimitInfoViewModel model);
        bool AmountTryParse(string modelAmount, out decimal amount);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;

namespace InternetBanking.Services
{
    public interface IBankService
    {
        Bank CreateBank(CreateBankViewModel model, BankInfo bankInfo);
        BankInfo CreateBankInfo(CreateBankViewModel model);
        Bank EditBank(int bankId, CreateBankViewModel model);
        BankInfo EditBankInfo(BankInfo bankInfo, CreateBankViewModel model);

        IQueryable<Bank> GetBankList();
        Bank GetBank(int id);
        Bank GetBankBik(string Bik);
        OurBank OurBank();

    }
}

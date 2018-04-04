using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class BankService : IBankService
    {
        private ApplicationContext context;

        public BankService(ApplicationContext context)
        {

            this.context = context;
        }
        public Bank CreateBank(CreateBankViewModel model, BankInfo bankInfo)
        {
            Bank bank = new Bank
            {  
                BIK = model.BIK,
                BankInfoId = bankInfo.Id
            };

            context.Banks.Add(bank);
            context.SaveChanges();
            return bank;
        }

        public BankInfo CreateBankInfo(CreateBankViewModel model)
        {
            BankInfo bankInfo = new BankInfo
            {
                Email = model.Email,
                BankName = model.BankName
            };
            context.BankInfos.Add(bankInfo);
            context.SaveChanges();
            return bankInfo;
        }

        public BankInfo EditBankInfo(BankInfo bankInfo, CreateBankViewModel model)
        {
            BankInfo info = context.BankInfos.FirstOrDefault(b => b.Id == bankInfo.Id);
            info.BankName = model.BankName;
            info.Email = model.Email;
            context.BankInfos.Update(info);
            context.SaveChanges();
            return bankInfo;
        }

        public Bank EditBank (int bankId, CreateBankViewModel model)
        {
            Bank bank = GetBank(bankId);
            bank.BIK = model.BIK;
            context.Banks.Update(bank);
            context.SaveChanges();
            return bank;
        }

        public IQueryable<Bank> GetBankList()
        {
            IQueryable<Bank> banks = context.Banks.Include(b => b.BankInfo);
            return banks;
        }

        public Bank GetBank(int id)
        {
            Bank bank = GetBankList().FirstOrDefault(b => b.Id == id);
            return bank;
        }
        public Bank GetBankBik(string Bik)
        {
            Bank bank = GetBankList().FirstOrDefault(b => b.BIK == Bik);
            return bank;
        }

        public OurBank OurBank()
        {
            OurBank bank = context.OurBank.Include(b => b.BankInfo).Include(a => a.Account).ThenInclude(c => c.Currency).FirstOrDefault();
            return bank;
        }

    }
}

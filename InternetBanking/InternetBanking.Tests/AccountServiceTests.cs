using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Moq;
using Xunit;

namespace InternetBanking.Tests
{
    public class AccountServiceTests
    {
        private ApplicationContext context;
        private  IAccountService accountService; 
        public AccountServiceTests()
        {
            context = TestServicesProvider.GetContext();
            TestServicesProvider.GetModelTestData().FillData();
            accountService = TestServicesProvider.GetAccountService();
        }


        [Fact]
        public void GenerationRandNumber()
        {

           
            string randNumber = accountService.GetUniqueAccountNumber();
            Assert.Equal(16, randNumber.Length);
        }
        
        [Fact]
        public void IsReturnedAccountNumberUnique()
        {

            
            string result = accountService.GetUniqueAccountNumber();
            Assert.Equal(16, result.Length);

            Assert.DoesNotContain(result, GetAccountNumbers());
        }
        
        [Fact]
        public void IsCreateAccountForUserNotNull()
        {
            
            Account account = accountService.CreateAccount(new User(), new Currency());
            Assert.NotNull(account);

        }
        [Fact]
        public void AddAccountCheckDb()
        {
            
            Currency currency = context.Currencies.FirstOrDefault();
            Account account = accountService.CreateAccount(new User(), currency);
            accountService.AddAccount(account);
            Account accountInDb = context.Accounts.FirstOrDefault(a => a.Id == account.Id);
            Assert.NotNull(accountInDb);

        }

        [Fact]
        public void IsCreateAccountForCompanyNotNull()
        {
           

            Account account = accountService.CreateAccount(new Company(), new Currency());
            Assert.NotNull(account);

        }
        
        private ICollection<string> GetAccountNumbers()
        {
            return context.Accounts.Select(a => a.Number).ToList();
        }


    }
}

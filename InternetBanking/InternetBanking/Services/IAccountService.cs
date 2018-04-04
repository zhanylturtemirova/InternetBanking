using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;

namespace InternetBanking.Services
{
    public interface IAccountService
    {
        string GetUniqueAccountNumber();

        Account CreateAccount(User user, Currency currency);
        
        Account CreateAccount(Company company, Currency currency);

        void AddAccount(Account account);

        List<AccountWithBalance> GetUserInfoAccounts(int id);

        List<AccountWithBalance> GetCompanyAccounts(int id);

        Task<Account> FindAccountByNumber(string accountNumber);

        Task<decimal> GetAccountBalance(Account account);

        Task<Account> FindAccountById(int accountId);

        EmployeeAccount FindEmployeeAccountByUserIdAndAccountId(string userId, int accountId);

        Task<bool> CompareAccountsCurrencies(string receiverAccountNumber, int senderAccountId);

        Task<bool> IsBalanceEnough(int accountId, decimal amount);
        Task<bool> IsAccountExist(string accountNumber);
        Task<bool> IsAccountSenderNotReceiver(string accountNumber, User user);
        List<Account> GetCompanyAccountsWithoutBalance(int id);
        List<Account> GetUserAccountsWithoutBalance(int id);
        bool AmountTryParse(string modelAmount, out decimal amount);
        Task<bool> IsUserHaveRightsOnAccount(User user, int accountId);

        Task<EditAccountViewModel> GetEditAccountViewModel(Account account);


        void UpdateAccount(EditAccountViewModel model);
        void BlockAccount(int accountId);
        void UnblockAccount(int accountId);

        Account OurBankAccount();

    }
}
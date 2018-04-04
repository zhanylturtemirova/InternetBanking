using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;

namespace InternetBanking.Services
{
    public class AccountService : IAccountService
    {
        private const string BANK_CODE = "123";
        private readonly ApplicationContext context;

        private readonly ISelectListService selectListService;

        private readonly IBankService bankService;

        public AccountService(ApplicationContext context, ISelectListService selectListService, IBankService bankService)
        {
            this.context = context;
            this.selectListService = selectListService;
            this.bankService = bankService;
        }

        public Account CreateAccount(User user, Currency currency)
        {
            UserInfo userInfo = context.UserInfo.FirstOrDefault(ui => ui.UserId == user.Id);
            Account account = new Account
            {
                Number = GetUniqueAccountNumber(),
                UserInfo = userInfo,
                Currency = currency,
                Locked = false

            };
            return account;

        }

        public Account CreateAccount(Company company, Currency currency)
        {
            Account account = new Account
            {
                Number = GetUniqueAccountNumber(),
                Company = company,
                Currency = currency,
                Locked = false

            };
            return account;

        }

        public async void AddAccount(Account account)
        {
            await context.Accounts.AddAsync(account);
            context.SaveChanges();
        }

        public string GetUniqueAccountNumber()
        {
            OurBank bank = bankService.OurBank();
            string number = string.Empty;
            bool isUnique = false;
            while (!isUnique)
            {
                number = GetAccountNumber(bank.BIK);
                isUnique = IsUniqueAsync(number).Result;
            }
            return number;
        }





        private string GenerateRandomNumber()
        {
            Random rnd = new Random();
            string number = string.Empty;
            for (int i = 0; i < 11; i++)
            {
                int n = rnd.Next(0, 10);
                number += n;
            }
            return number;
        }

        private string GetAccountNumber(string bankCode)
        {
            string number = bankCode + GenerateRandomNumber();
            string suffix = GetRemainder(number);
            string result = number + suffix;
            return result;
        }

        private string GetRemainder(string number)
        {
            string remainder = (long.Parse(number) % 97).ToString();
            if (remainder == "0")
            {
                return "97";
            }
            if (remainder.Length == 1)
            {
                return "0" + remainder;
            }
            return remainder;
        }

        private async Task<bool> IsUniqueAsync(string number)
        {
            Account account = await context.Accounts.FirstOrDefaultAsync(a => a.Number == number);
            return account == null;
        }

        public List<AccountWithBalance> GetUserInfoAccounts(int id)
        {
            List<Account> accounts = context.Accounts.Where(u => u.UserInfoId == id).Include(u => u.Currency).ToList();
            List<AccountWithBalance> accountWithBalances = new List<AccountWithBalance>();
            foreach (var acc in accounts)
            {
                accountWithBalances.Add(new AccountWithBalance
                {
                    Account = acc,
                    Balance = GetAccountBalance(acc).Result

                });
            }
            return accountWithBalances;
        }

        public List<AccountWithBalance> GetCompanyAccounts(int id)
        {
            List<Account> accounts = context.Accounts.Where(u => u.CompanyId == id).Include(u => u.Currency).ToList();
            List<AccountWithBalance> accountWithBalances = new List<AccountWithBalance>();
            foreach (var acc in accounts)
            {
                accountWithBalances.Add(new AccountWithBalance
                {
                    Account = acc,
                    Balance = GetAccountBalance(acc).Result

                });
            }
            return accountWithBalances;
        }

        public List<Account> GetCompanyAccountsWithoutBalance(int id)
        {
            List<Account> accounts = context.Accounts.Where(u => u.CompanyId == id).Include(u => u.Currency).ToList();

            return accounts;
        }

        public List<Account> GetUserAccountsWithoutBalance(int id)
        {
            List<Account> accounts = context.Accounts.Where(u => u.UserInfoId == id).Include(u => u.Currency).ToList();

            return accounts;
        }

        public async Task<Account> FindAccountByNumber(string accountNumber)
        {
            Account account = await context.Accounts.FirstOrDefaultAsync(a => a.Number == accountNumber);
            return account;
        }

        public async Task<decimal> GetAccountBalance(Account account)
        {
            decimal balance = await context.Transactions
                                 .Where(t => t.TransactionType.TypeName == TransactionTypesEnum.Debit.ToString() && t.AccountId == account.Id)
                                 .SumAsync(t => t.Amount) -
                             await context.Transactions
                                 .Where(t => t.TransactionType.TypeName == TransactionTypesEnum.Credit.ToString() && t.AccountId == account.Id)
                                 .SumAsync(t => t.Amount);
            return Math.Round(balance, 2);
        }

        public async Task<Account> FindAccountById(int accountId)
        {
            Account account = await context.Accounts.Include(a => a.Currency).Include(u=>u.UserInfo).ThenInclude(a=>a.User).FirstOrDefaultAsync(a => a.Id == accountId);
            return account;
        }

        public async Task<bool> IsAccountExist(string accountNumber)
        {
            Account account = await context.Accounts.FirstOrDefaultAsync(a => a.Number == accountNumber);
            return account != null;
        }

        public async Task<bool> CompareAccountsCurrencies(string receiverAccountNumber, int senderAccountId)
        {
            Account receiver = await FindAccountByNumber(receiverAccountNumber);
            Account sender = await FindAccountById(senderAccountId);
            return receiver.CurrencyId != sender.CurrencyId;
        }

        public async Task<bool> IsBalanceEnough(int accountId, decimal amount)
        {
            Account account = await FindAccountById(accountId);
            decimal balance = await GetAccountBalance(account);
            return amount < balance;
        }

        public async Task<bool> IsAccountSenderNotReceiver(string accountNumber, User user)
        {
            UserInfo userInfo = await context.UserInfo.FirstOrDefaultAsync(ui => ui.UserId == user.Id);
            if (userInfo == null)
            {
                EmployeeInfo employeeInfo = await context.EmployeeInfos.FirstOrDefaultAsync(ei => ei.UserId == user.Id);
                return context.Accounts.Where(a => a.CompanyId == employeeInfo.CompanyId).ToList()
                    .Exists(a => a == FindAccountByNumber(accountNumber).Result);
            }
            
            List<Account> userAccounts = await context.Accounts.Where(a => a.UserInfoId == userInfo.Id).ToListAsync();
            Account account = await FindAccountByNumber(accountNumber);
            return userAccounts.Exists(a =>a == account);
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

        public EmployeeAccount FindEmployeeAccountByUserIdAndAccountId(string userId, int accountId)
        {  
            UserInfo userInfo = context.UserInfo.FirstOrDefault(u => u.UserId == userId);
            EmployeeInfo employee = context.EmployeeInfos.FirstOrDefault(u => u.UserId == userId);
            if(employee != null)
            {
                EmployeeAccount accountEmployee = context.EmployeeAccounts.Include(l => l.limit).FirstOrDefault(a => a.EmployeeId == employee.Id && a.AccountId == accountId);
                return accountEmployee;
            }
            EmployeeAccount accountUser = context.EmployeeAccounts.Include(l => l.limit).FirstOrDefault(a => a.UserId == userInfo.Id && a.AccountId == accountId);
            return accountUser;
        }

        public async Task<bool> IsUserHaveRightsOnAccount(User user, int accountId)
        {
            Account account = await FindAccountById(accountId);
            if (account == null)
            {
                return false;
            }
            EmployeeInfo employee = await context.EmployeeInfos.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employee != null)
            {
                if (employee.Chief && account.CompanyId == employee.CompanyId)
                {
                    return true;
                }
                EmployeeAccount employeeAccount = await context.EmployeeAccounts.FirstOrDefaultAsync(ea => ea.EmployeeId == employee.Id && ea.AccountId == account.Id);
                if (employeeAccount != null)
                {
                    return employeeAccount.RightOfCreate;
                }
                return false;
            }
            UserInfo userInfo = await context.UserInfo.FirstOrDefaultAsync(ui => ui.UserId == user.Id);
            if (userInfo != null)
            {
                return account.UserInfoId == userInfo.Id;
            }
            return false;
        }

        public async Task<EditAccountViewModel> GetEditAccountViewModel(Account account)
        {
            var limits = selectListService.GetLimits();
            EditAccountViewModel model = new EditAccountViewModel
                {
                    AccountId = account.Id,
                    UserAccounts = await context.EmployeeAccounts
                        .Include(ea => ea.Account).Include(ea => ea.Employee).Include(ea => ea.User)
                        .Where(ea => ea.AccountId == account.Id).Select(ea =>
                            new EmployeeAccountViewModel
                            {
                                Account = ea.Account,
                                Employee = ea.Employee,
                                EmployeeId = ea.EmployeeId,
                                UserInfoId = ea.UserId,
                                UserInfo = ea.User,
                                Limits = limits,
                                LimitId = ea.LimitId,
                                RightOfConfirmation = ea.RightOfConfirmation,
                                RightOfCreate = ea.RightOfCreate
                            }).ToListAsync()


                };




            return model;
        }

        public void UpdateAccount(EditAccountViewModel model)
        {
            List<EmployeeAccount> employeeAccounts =
                context.EmployeeAccounts.Where(ea => ea.AccountId == model.AccountId).ToList();
            foreach (var employeeAccount in employeeAccounts)
            {
                if (employeeAccount.EmployeeId != null)
                {
                    var empAcc = model.UserAccounts
                        .FirstOrDefault(ua => ua.EmployeeId == employeeAccount.EmployeeId);
                    employeeAccount.LimitId = empAcc.LimitId;
                    employeeAccount.RightOfCreate = empAcc.RightOfCreate;
                    employeeAccount.RightOfConfirmation = empAcc.RightOfConfirmation;
                }
                else
                {
                    employeeAccount.LimitId = model.UserAccounts
                        .FirstOrDefault(ua => ua.UserInfoId == employeeAccount.UserId).LimitId;

                }
               
            }
            context.UpdateRange(employeeAccounts);
            context.SaveChanges();
        }

        public void BlockAccount(int accountId)
        {
            Account account = context.Accounts.FirstOrDefault(a => a.Id == accountId);
            account.Locked = true;
            context.Accounts.Update(account);
            context.SaveChanges();
        }

        public void UnblockAccount(int accountId)
        {
            Account account = context.Accounts.FirstOrDefault(a => a.Id == accountId);
            account.Locked = false;
            context.Accounts.Update(account);
            context.SaveChanges();
        }

        public Account OurBankAccount()
        {
            OurBank bank = bankService.OurBank();
            Account account = bank.Account;
            return account;

        }
    }
}

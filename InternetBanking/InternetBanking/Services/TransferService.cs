using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class TransferService : ITransferService
    {
        private readonly ApplicationContext context;
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;
        private readonly ISelectListService selectListService;
        private readonly IAccountService accountService;

        public TransferService(ApplicationContext context, ITransactionService transactionService, IUserService userService, ISelectListService selectListService, IAccountService accountService)
        {
            this.context = context;
            this.transactionService = transactionService;
            this.userService = userService;
            this.selectListService = selectListService;
            this.accountService = accountService;
        }

        private async Task<TransferState> FindTransferStateByName(TransferStatesEnum enumItem)
        {
            return await context.TransferStates.FirstOrDefaultAsync(t => t.IsEqual(enumItem));
        }

        public InnerTransfer FindInnerTransferById(int innerTransferId)
        {
            InnerTransfer transfer = context.InnerTransfers.FirstOrDefault(t => t.Id == innerTransferId);
            return transfer;
        }

        public InterBankTransfer FindInterBankTransferById(int transferId)
        {
            InterBankTransfer transfer = context.InterBankTransfers.Include(it => it.Transfer).FirstOrDefault(t => t.Id == transferId);
            return transfer;
        }

        public async void AddTransfer(InnerTransfer transfer)
        {
            if (transfer.AccountSender != null && !transfer.AccountSender.Locked)
            {
                await context.AddAsync(transfer);
                if (transfer.TransferState.IsEqual(TransferStatesEnum.Confirmed))
                {
                    List<Transaction> transactions = transactionService.CreateTransactions(transfer);
                    transactionService.AddTransactions(transactions);
                }
                context.SaveChanges();
            }
            if (transfer.AccountSender == null)
            {
                await context.AddAsync(transfer);
                if (transfer.TransferState.IsEqual(TransferStatesEnum.Confirmed))
                {

                    List<Transaction> transactions = transactionService.CreateTransactions(transfer);
                    transactionService.AddTransactions(transactions);
                }
                context.SaveChanges();
            }
        }

        public async Task<InnerTransfer> CreateInnerTransfer(Account sender, Account receiver, decimal amount, string comment,
            decimal? amountReceive, int? exchangeRateId, int? exchangeRateIdSecond)
        {
            TransferState state = await FindTransferStateByName(TransferStatesEnum.Confirmed);
            if (sender != null)
            {
                if (sender.UserInfoId == null)
                {
                    state = await FindTransferStateByName(TransferStatesEnum.NotConfirmed);
                }
                if (!await accountService.IsBalanceEnough(sender.Id, amount))
                {
                    state = await FindTransferStateByName(TransferStatesEnum.BalanceNotEnough); 
                }
                if (sender.Locked)
                {
                    state = await FindTransferStateByName(TransferStatesEnum.AccountIsLocked); 
                }
               
            }

            
            InnerTransfer transfer = new InnerTransfer
            {
                AccountSender = sender,
                AccountReceiver = receiver,
                Amount = amount,

                Comment = comment,
                TransferState = state,
                TransferDate = DateTime.Now,

                AmountReceive = amountReceive,
                ExchangeRateId = exchangeRateId,
                ExchangeRateIdSecond = exchangeRateIdSecond

            };
            return transfer;
        }


        public  bool GetEmployeeRightOfConfirm(EmployeeInfo employee, Account account)
        {
            if (account == null)
            {
                return false;
            }
            if (employee.Chief && account.CompanyId == employee.CompanyId)
            {
                return true;
            }
            EmployeeAccount employeeAccount =  context.EmployeeAccounts.FirstOrDefault(ea => ea.EmployeeId == employee.Id && ea.AccountId == account.Id);
            if (employeeAccount != null)
            {
                return employeeAccount.RightOfConfirmation;
            }
            return false;
        }
        

        public IQueryable<ConfirmTransferViewModel> GetAllTransferViewModelsByCompanyId(User user)
        {
            EmployeeInfo employee = context.EmployeeInfos.FirstOrDefault(ei => ei.UserId == user.Id);
            if (employee != null)
            {
                IQueryable<ConfirmTransferViewModel> transfers = context.InnerTransfers
                    .Where(it => ( it.AccountSender.CompanyId == employee.CompanyId) && it.AccountSenderId != null)
                    .Include(t => t.AccountSender).Include(t => t.AccountReceiver).Include(t => t.TransferState)
                    .OrderByDescending(t => t.TransferDate)
                    .Select(t => new ConfirmTransferViewModel
                    {
                        Transfer = t,
                        TransferId = t.Id
                    });

                return transfers;
            }
            else
            {
                UserInfo userInfo = context.UserInfo.FirstOrDefault(ui => ui.UserId == user.Id);
                IQueryable<ConfirmTransferViewModel> transfers = context.InnerTransfers
                    .Where(it => (it.AccountSender.UserInfoId == userInfo.Id) && it.AccountSenderId != null)
                    .Include(t => t.AccountSender).Include(t => t.AccountReceiver).Include(t => t.TransferState)
                    .OrderByDescending(t => t.TransferDate)
                    .Select(t => new ConfirmTransferViewModel
                    {
                        Transfer = t,
                        TransferId = t.Id
                    });

                return transfers;
            }

        }

        public virtual IQueryable<ConfirmTransferViewModel> GetNotConfirmedTransferViewModelsByCompanyId(User user)
        {

            EmployeeInfo employee = context.EmployeeInfos.FirstOrDefault(ei => ei.UserId == user.Id);
            if (employee != null)
            {
                IQueryable<ConfirmTransferViewModel> transfers = context.InnerTransfers
                    .Where(it =>
                        it.TransferState.StateName == TransferStatesEnum.NotConfirmed.ToString() && it.AccountSenderId != null &&
                        (it.AccountReceiver.CompanyId == employee.CompanyId || it.AccountSender.CompanyId == employee.CompanyId))
                    .Include(t => t.AccountSender).Include(t => t.AccountReceiver).Include(t => t.TransferState)
                    .OrderByDescending(t => t.TransferDate)
                    .Select(t => new ConfirmTransferViewModel
                    {
                        Transfer = t,
                        TransferId = t.Id
                    });

                return transfers;
            }
            else
            {
                UserInfo userInfo = context.UserInfo.FirstOrDefault(ui => ui.UserId == user.Id);
                IQueryable<ConfirmTransferViewModel> transfers = context.InnerTransfers
                    .Where(it => it.TransferState.StateName == TransferStatesEnum.NotConfirmed.ToString() && it.AccountSenderId != null &&
                        (it.AccountReceiver.UserInfoId == userInfo.Id || it.AccountSender.UserInfoId == userInfo.Id))
                    .Include(t => t.AccountSender).Include(t => t.AccountReceiver).Include(t => t.TransferState)
                    .OrderByDescending(t => t.TransferDate)
                    .Select(t => new ConfirmTransferViewModel
                    {
                        Transfer = t,
                        TransferId = t.Id
                    });

                return transfers;
            }
           

        }

        public async Task<InnerTransfer> FindTransferById(int transferId)
        {
            InnerTransfer transfer = await context.InnerTransfers.Include(t=>t.TransferState)
                 .Include(t=>t.AccountSender).Include(t => t.AccountReceiver).FirstOrDefaultAsync(t => t.Id == transferId);
            return transfer;
        }

        public async  Task ConfirmTransfer(InnerTransfer transfer)
        {
            TransferState state = await FindTransferStateByName(TransferStatesEnum.NotConfirmed);
            if (transfer.TransferStateId == state.Id)
            {
                List<Transaction> transactions = transactionService.CreateTransactions(transfer);
                transactionService.AddTransactions(transactions);
                transfer.TransferState = await FindTransferStateByName(TransferStatesEnum.Confirmed);
                transfer.TransferDate = DateTime.Now;
                context.InnerTransfers.Update(transfer);
                context.SaveChanges();
            }            
        }

        public async Task CancelTransfer(int transferId)
        {
            InnerTransfer transfer = context.InnerTransfers.FirstOrDefault(t => t.Id == transferId);
            if (transfer != null)
            {
                transfer.TransferState = await FindTransferStateByName(TransferStatesEnum.Canceled); // context.TransferStates.FirstOrDefault(s => s.IsEqual(TransferStatesEnum.Canceled)).Id;
                transfer.TransferDate = DateTime.Now;
                context.InnerTransfers.Update(transfer);
                context.SaveChanges();
            }
        }
        

        public async Task<List<StatementObjectViewModel>> GetAccountTransfers(int accountId)
        {
            //IQueryable<StatementObjectViewModel> transfers = context.InnerTransfers
            //    .Where(t => t.AccountReceiverId == accountId || t.AccountSenderId == accountId)
               

            List<InnerTransfer> transfers = await context.InnerTransfers
                .Include(t => t.AccountReceiver).ThenInclude(a => a.Currency)
                .Include(t => t.ExchangeRate).ThenInclude(r => r.Currency)
                .Include(t => t.ExchangeSecond).ThenInclude(r => r.Currency)
                .Include(t => t.AccountSender).ThenInclude(a => a.Currency)
                .Where(t => t.AccountReceiverId == accountId || t.AccountSenderId == accountId && t.TransferState.StateName == TransferStatesEnum.Confirmed.ToString()).OrderByDescending(t => t.TransferDate).ToListAsync();
            List<StatementObjectViewModel> models = new List<StatementObjectViewModel>();
            foreach (var innerTransfer in transfers)
            {
                InterBankTransfer interTranfer =
                    await context.InterBankTransfers.FirstOrDefaultAsync(it => it.InnerTransferId == innerTransfer.Id);
                if (interTranfer != null)
                {                  
                    StatementObjectViewModel statementObjectViewModel = new StatementObjectViewModel
                    {
                        AccountNumber = interTranfer.AccountNumber + innerTransfer.AccountSender.Currency.Name,
                        Comment = innerTransfer.Comment,
                        CreditAmount = innerTransfer.Amount.ToString() ,
                        TransferDate = innerTransfer.TransferDate,                       
                    };
                    models.Add(statementObjectViewModel);
                }
                else
                {
                    if (innerTransfer.AccountReceiverId == accountId)
                    {
                        
                        if (innerTransfer.ExchangeRateId != null)
                        {
                            StatementObjectViewModel statementObjectViewModel = new StatementObjectViewModel
                            {   
                                AccountNumber = innerTransfer.AccountSender.Number + innerTransfer.AccountSender.Currency.Name,
                                Comment = innerTransfer.Comment,
                                DebitAmount = innerTransfer.AmountReceive.ToString(),
                                TransferDate = innerTransfer.TransferDate

                            };
                            if (innerTransfer.AccountReceiver.Currency.IsNativeCurrency)
                            {
                                statementObjectViewModel.Rate =
                                    innerTransfer.ExchangeRate.Currency.Name + " " +
                                    innerTransfer.ExchangeRate.RateForPurchaise + "/" +
                                    innerTransfer.ExchangeRate.RateForSale;
                            }
                            else
                            {
                                statementObjectViewModel.Rate =
                                    innerTransfer.ExchangeRate.Currency.Name + " " +
                                    innerTransfer.ExchangeRate.RateForPurchaise + "/" +
                                    innerTransfer.ExchangeRate.RateForSale;
                            }
                            models.Add(statementObjectViewModel);
                        }

                        else
                        {
                            if (innerTransfer.AccountSender != null)
                            {
                                StatementObjectViewModel statementObjectViewModel = new StatementObjectViewModel
                                {
                                    AccountNumber = innerTransfer.AccountSender.Number + innerTransfer.AccountSender.Currency.Name,
                                    TransferDate = innerTransfer.TransferDate,
                                    Comment = innerTransfer.Comment,
                                    DebitAmount = innerTransfer.Amount.ToString(),
                                };
                                models.Add(statementObjectViewModel);
                            }
                            else
                            {
                                StatementObjectViewModel statementObjectViewModel = new StatementObjectViewModel
                                {
                                    AccountNumber = "",
                                    TransferDate = innerTransfer.TransferDate,
                                    Comment = innerTransfer.Comment,
                                    DebitAmount = innerTransfer.Amount.ToString(),
                                };
                                models.Add(statementObjectViewModel);
                            }                                                        
                        }
                    }
                    else
                    {
                        StatementObjectViewModel statementObjectViewModel = new StatementObjectViewModel
                        {
                            AccountNumber = innerTransfer.AccountSender.Number + innerTransfer.AccountSender.Currency.Name,
                            Comment = innerTransfer.Comment,
                            CreditAmount = innerTransfer.Amount.ToString(),
                            TransferDate = innerTransfer.TransferDate
                            
                        };
                        if (innerTransfer.ExchangeRateId != null)
                        {
                           
                            if (innerTransfer.AccountSender.Currency.IsNativeCurrency != true && innerTransfer.ExchangeSecond != null)
                            {
                                statementObjectViewModel.Rate =
                                    innerTransfer.ExchangeSecond.Currency.Name + " " +
                                    innerTransfer.ExchangeSecond.RateForPurchaise + "/" +
                                    innerTransfer.ExchangeSecond.RateForSale;
                            }
                            else
                            {
                                statementObjectViewModel.Rate =
                                    innerTransfer.ExchangeRate.Currency.Name + " " +
                                    innerTransfer.ExchangeRate.RateForPurchaise + "/" +
                                    innerTransfer.ExchangeRate.RateForSale;
                            }
                            models.Add(statementObjectViewModel);
                        }
                        else
                        {                          
                            models.Add(statementObjectViewModel);
                        }
                    }
                }
            }


            return models;
        }


        public InterBankTransfer CreateInterBankTransfer( InterBankTransferViewModel model, InnerTransfer innerTransfer)
        {
            if (!innerTransfer.AccountSender.Locked)
            {
                InterBankTransfer transfer = new InterBankTransfer
                {
                    AccountNumber = model.Transfer.ReceiverAccountNumber,
                    ReciverName = model.ReciverName,
                    BankId = model.BankId,
                    PaymentCodeId = model.PaymentCodeId,
                    InnerTransferId = innerTransfer.Id


                };
                context.Add(transfer);
                context.SaveChanges();
                return transfer;
            }
            return null;

        }

        public InnerTransferViewModel GetMethodInnerTransfer(User user, InnerTransferViewModel model)
        {
            string userName = String.Empty;
            int userId = 0;
            UserInfo userInfo = userService.FindUserByIdInUserInfo(user.Id, ref userName, ref userId);
            EmployeeInfo employeeInfo = userService.FindUserByIdInCompany(user.Id, ref userName, ref userId);
            
            if (userInfo != null)
            {
                model.UserAccounts = selectListService.GetUserAccounts(userInfo.Id);
            }
            if (employeeInfo != null)
            {
                model.UserAccounts = selectListService.GetEmployeeAccounts(employeeInfo.Id);
            }

            model.SaveInTempalte = false;

            return model;
        }

        
    }
}

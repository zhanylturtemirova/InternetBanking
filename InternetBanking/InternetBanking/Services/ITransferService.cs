using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;

namespace InternetBanking.Services
{
    public interface ITransferService
    {
        void AddTransfer(InnerTransfer transfer);
        Task<InnerTransfer> CreateInnerTransfer(Account sender, Account receiver, decimal amount, string comment, decimal? amountReceive, int? exchangeRateId, int? exchangeRateIdSecond);
      
        bool GetEmployeeRightOfConfirm(EmployeeInfo employee, Account account);

        IQueryable<ConfirmTransferViewModel> GetNotConfirmedTransferViewModelsByCompanyId(User user);

        IQueryable<ConfirmTransferViewModel> GetAllTransferViewModelsByCompanyId(User user);
        Task<InnerTransfer> FindTransferById(int transferId);
        Task ConfirmTransfer(InnerTransfer transfer);
        Task CancelTransfer(int transferId);

        InterBankTransfer CreateInterBankTransfer(InterBankTransferViewModel model, InnerTransfer innerTransfer);

        Task<List<StatementObjectViewModel>> GetAccountTransfers(int accountId);

        InnerTransfer FindInnerTransferById(int innerTransferId);
        InterBankTransfer FindInterBankTransferById(int transferId);
        InnerTransferViewModel GetMethodInnerTransfer(User user, InnerTransferViewModel model);


    }
}

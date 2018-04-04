using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;

namespace InternetBanking.Services
{
    public interface ITransactionService
    {
        List<Transaction> CreateTransactions(InnerTransfer transfer);
        void AddTransactions(List<Transaction> transactions);
    }
}

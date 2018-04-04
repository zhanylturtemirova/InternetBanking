using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationContext context;


        public TransactionService(ApplicationContext context)
        {
            this.context = context;
        }

        private TransactionType FindTransactionTypeByName(TransactionTypesEnum enumItem)
        {
            return context.TransactionTypes.FirstOrDefault(t => t.IsEqual(enumItem));
        }

        

        public List<Transaction> CreateTransactions(InnerTransfer transfer)
        {
            
            List<Transaction> transactions = new List<Transaction>();
            if (transfer.AmountReceive == null)
            {
                if (transfer.AccountReceiverId != null)
                {

                    Transaction transaction = new Transaction
                    {
                        AccountId = (int) transfer.AccountReceiverId,
                        Amount = transfer.Amount,
                        TransactionTypeId = FindTransactionTypeByName(TransactionTypesEnum.Debit).Id,
                        DateOfTransaction = DateTime.Now

                    };
                    transactions.Add(transaction);
                }
                if (transfer.AccountSenderId != null)
                {

                    Transaction transaction = new Transaction
                    {
                        AccountId = (int) transfer.AccountSenderId,
                        Amount = transfer.Amount,
                        TransactionTypeId = FindTransactionTypeByName(TransactionTypesEnum.Credit).Id,
                        DateOfTransaction = DateTime.Now
                    };
                    transactions.Add(transaction);
                }
            }
            else
            {
                Transaction transactionReceive = new Transaction
                {
                    AccountId = (int)transfer.AccountReceiverId,
                    Amount = (decimal)transfer.AmountReceive,
                    TransactionTypeId = FindTransactionTypeByName(TransactionTypesEnum.Debit).Id,
                    DateOfTransaction = DateTime.Now

                };
                transactions.Add(transactionReceive);


                Transaction transactionSend = new Transaction
                {
                    AccountId = (int)transfer.AccountSenderId,
                    Amount =(decimal) transfer.Amount,
                    TransactionTypeId = FindTransactionTypeByName(TransactionTypesEnum.Credit).Id,
                    DateOfTransaction = DateTime.Now

                };
                transactions.Add(transactionSend);

            }

            return transactions;
        }

        public void AddTransactions(List<Transaction> transactions)
        {
            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}

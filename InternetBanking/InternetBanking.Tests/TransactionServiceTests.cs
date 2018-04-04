using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels.Enums;
using Xunit;

namespace InternetBanking.Tests
{
    public class TransactionServiceTests
    {
        private readonly ApplicationContext context;
        private ITransactionService transactionService;
        public TransactionServiceTests()
        {
            TestServicesProvider.GetModelTestData().FillData();
            context = TestServicesProvider.GetContext();
            transactionService = TestServicesProvider.GetTransactionService();
        }

        [Fact]
        public void CreateTransactionsPropertiesValid()
        {
            
            Account accountReceiver = new Account{Id = 1};
            Account accountSender = new Account{Id = 2};
            InnerTransfer transfer = new InnerTransfer
            {
                AccountReceiverId = accountReceiver.Id,
                AccountSenderId =  accountSender.Id,
                Amount = 100,
                Comment = "No Comments"
            };
            List<Transaction> transactions = transactionService.CreateTransactions(transfer);
            Assert.True(2 == transactions.Count);
            Assert.Equal(transactions[0].AccountId, transfer.AccountReceiverId);
            Assert.Equal(transactions[1].AccountId, transfer.AccountSenderId);
            Assert.Equal(context.TransactionTypes.FirstOrDefault(t=>t.IsEqual(TransactionTypesEnum.Debit)).Id, transactions[0].TransactionTypeId);
            Assert.Equal(context.TransactionTypes.FirstOrDefault(t => t.IsEqual(TransactionTypesEnum.Credit)).Id, transactions[1].TransactionTypeId);
        }

        [Fact]
        public void CreateTransactionsCreditOnlyPropertiesValid()
        {
            InnerTransfer transfer = new InnerTransfer
            {
                
                AccountSenderId = 2,
                Amount = 100,
                Comment = "No Comments"
            };
            List<Transaction> transactions = transactionService.CreateTransactions(transfer);
            Assert.True(transactions.Count == 1);   
            Assert.Equal(transactions[0].AccountId, transfer.AccountSenderId);
            Assert.Equal(2, transactions[0].TransactionTypeId);
        }

        [Fact]
        public void CreateTransactionsDebitOnlyPropertiesValid()
        {
            
            InnerTransfer transfer = new InnerTransfer
            {
                AccountReceiverId = 1,
                Amount = 100,
                Comment = "No Comments"
            };
            List<Transaction> transactions = transactionService.CreateTransactions(transfer);
            Assert.True(transactions.Count ==1);
            Assert.Equal(transactions[0].AccountId, transfer.AccountReceiverId);
            Assert.Equal(1, transactions[0].TransactionTypeId);
        }

        [Fact]
        public void CheckCreatedTransactions()
        {
           
            InnerTransfer transfer = new InnerTransfer
            {
                AccountReceiverId = 1,
                AccountSenderId = 2,
                Amount = 100,
                Comment = "No Comments"
            };
            List<Transaction> transactions = transactionService.CreateTransactions(transfer);
            transactionService.AddTransactions(transactions:transactions);
            Assert.NotNull(context.Transactions.FirstOrDefault(t => t == transactions[0]));
            Assert.NotNull(context.Transactions.FirstOrDefault(t => t == transactions[1]));
        }

        [Fact]
        public void CheckCreatedTransactionsDebitOnly()
        {
           
            InnerTransfer transfer = new InnerTransfer
            {
                AccountReceiverId = 1,
                Amount = 300,
                Comment = "No Comments"
            };
            List<Transaction> transactions = transactionService.CreateTransactions(transfer);
            transactionService.AddTransactions(transactions: transactions);
            Assert.NotNull(context.Transactions.FirstOrDefault(t => t == transactions[0]));
        }
        [Fact]
        public void CheckCreatedTransactionsCreditOnly()
        {
            ITransactionService service = new TransactionService(context);
            InnerTransfer transfer = new InnerTransfer
            {
                AccountSenderId = 2,
                Amount = 500,
                Comment = "No Comments"
            };
            List<Transaction> transactions = service.CreateTransactions(transfer);
            service.AddTransactions(transactions: transactions);
            Assert.NotNull(context.Transactions.FirstOrDefault(t => t == transactions[0]));

        }

    }
}

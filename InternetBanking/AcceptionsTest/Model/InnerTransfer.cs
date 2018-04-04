using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;


namespace AcceptionsTest.Model
{
    public class InnerTransfer 
    {

        [Key]
        public int Id { get; set;}

        public int? AccountSenderId { get; set; }
        public Account AccountSender { get; set; }

        public int?  AccountReceiverId { get; set; }
        public Account AccountReceiver { get; set; }

        public decimal Amount { get; set; }
        public decimal? AmountReceive { get; set; }
        
        public string Comment { get; set; }

        public int TransferStateId { get; set; }
        public TransferState TransferState { get; set; }

        public DateTime TransferDate { get; set; }

        public int? ExchangeRateId { get; set; }
        public ExchangeRate ExchangeRate { get; set; }

        public int? ExchangeRateIdSecond { get; set; }
        [ForeignKey("ExchangeRateIdSecond")]
        public ExchangeRate ExchangeSecond { get; set; }


        public InnerTransfer(int id, int? accountSenderId, Account accountSender, int? accountReceiverId, Account accountReceiver, decimal amount, decimal? amountReceive, string comment, int transferStateId, TransferState transferState, DateTime transferDate, int? exchangeRateId, ExchangeRate exchangeRate, int? exchangeRateIdSecond, ExchangeRate exchangeSecond)
        {
            Id = id;
            AccountSenderId = accountSenderId;
            AccountSender = accountSender;
            AccountReceiverId = accountReceiverId;
            AccountReceiver = accountReceiver;
            Amount = amount;
            AmountReceive = amountReceive;
            Comment = comment;
            TransferStateId = transferStateId;
            TransferState = transferState;
            TransferDate = transferDate;
            ExchangeRateId = exchangeRateId;
            ExchangeRate = exchangeRate;
            ExchangeRateIdSecond = exchangeRateIdSecond;
            ExchangeSecond = exchangeSecond;
        }

        public InnerTransfer()
        {
        }

       
    }
}

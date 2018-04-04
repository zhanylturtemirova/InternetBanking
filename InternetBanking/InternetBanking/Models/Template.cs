using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models.SelectTable;

namespace InternetBanking.Models
{
    public class Template
    {
        public int Id { get; set; }

        public string TempalteName { get; set; }

        public string TemplateDiscription { get; set; }

        public int? AccountSenderId { get; set; }
        public Account AccountSender { get; set; }

        public int? AccountReceiverId { get; set; }
        public Account AccountReceiver { get; set; }

        public decimal Amount { get; set; }       

        public string Comment { get; set; }

        public string AccountNumber { get; set; }

        public string ReciverName { get; set; }

        public int? BankId { get; set; }
        public Bank Bank { get; set; }

        public int? PaymentCodeId { get; set; }
        public PaymentСode PaymentСode { get; set; }

        public int? UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? TypeOfTransferId { get; set; }
        public TypeOfTransfer Type { get; set; }

    }
}

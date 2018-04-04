using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class OurBank
    {
        public int Id { get; set; }

        public int BankInfoId { get; set; }
        public BankInfo BankInfo { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string BIK { get; set; }
    }
}

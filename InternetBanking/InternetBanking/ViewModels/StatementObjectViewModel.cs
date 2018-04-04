using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class StatementObjectViewModel
    {
        public int Number { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransferDate { get; set; }
        public string DebitAmount { get; set; }
        public string CreditAmount { get; set; }
        public string Comment { get; set; }
        public string Rate { get; set; }
        public string Rate2 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class PdfForExactTransferViewModel
    {
        public string FileName { get; set; }
        public string AccountSender { get; set; }
        public string AccountReceiver { get; set; }
        public string TransferDate { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string State { get; set; }
    }
}

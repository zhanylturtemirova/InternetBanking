using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels.Paging;

namespace InternetBanking.ViewModels
{
    public class AccountTransfersViewModel
    {
        public List<StatementObjectViewModel> Transfers { get; set; }
        public List<ExchangeRate> Rates { get; set; }
        public Account Account { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FullName { get; set; }
        public string Excel { get; set; }
    }
}

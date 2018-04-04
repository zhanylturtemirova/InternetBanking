using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class ConvertConversionToDocViewModel
    {
        public string AccountToNumber { get; set; }
        public string AccountFromNumber { get; set; }
        public string CurrencyFromName { get; set; }
        public string CurrencyToName { get; set; }
        public string AmountSend { get; set; }
        public string AmountReceive { get; set; }
        public string CurrencyFromRate { get; set; }
        public string CurrencyToRate { get; set; }
        public string Date { get; set; }
        
    }
}

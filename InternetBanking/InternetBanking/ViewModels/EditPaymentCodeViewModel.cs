using InternetBanking.Models.SelectTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class EditPaymentCodeViewModel
    {
        public PaymentСode PaymentCode { get; set; }
        public PaymentCodeInfoViewModel PaymentCodeInfo { get; set; }
    }
}

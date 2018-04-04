using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public interface IPaymentCodeService
    {
        PaymentСode CreatePaymentСode(PaymentCodeInfoViewModel model);
        IQueryable<PaymentСode> GetPaymentСodeList();
        PaymentСode FindPaymentСodeId(int id);
        PaymentСode FindPaymentСodeIsCode(string Code);
        PaymentСode EditPaymentСode(int paymentСodeId, PaymentCodeInfoViewModel model);
    }
}

using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public class PaymentCodeService : IPaymentCodeService
    {
        private ApplicationContext context;

        public PaymentCodeService(ApplicationContext context)
        {
            this.context = context;
        }
        public PaymentСode CreatePaymentСode(PaymentCodeInfoViewModel model)
        {
            PaymentСode payment = new PaymentСode
            {
                Code = model.Code,
                PaymentCodeName = model.PaymentCodeName
            };

            context.PaymentСodies.Add(payment);
            context.SaveChanges();
            return payment;
        }
        public IQueryable<PaymentСode> GetPaymentСodeList()
        {
            IQueryable<PaymentСode> paymentСodes = context.PaymentСodies;
            return paymentСodes;
        }

        public PaymentСode FindPaymentСodeId(int id)
        {
            PaymentСode payment = GetPaymentСodeList().FirstOrDefault(c => c.Id == id);
            return payment;
        }
        public PaymentСode FindPaymentСodeIsCode(string Code)
        {
            PaymentСode payment = GetPaymentСodeList().FirstOrDefault(c => c.Code == Code);
            return payment;
        }

        public PaymentСode EditPaymentСode(int paymentСodeId, PaymentCodeInfoViewModel model)
        {
            PaymentСode payment = FindPaymentСodeId(paymentСodeId);
            payment.Code = model.Code;
            payment.PaymentCodeName = model.PaymentCodeName;

            context.PaymentСodies.Update(payment);
            context.SaveChanges();
            return payment;
        }
    }
}

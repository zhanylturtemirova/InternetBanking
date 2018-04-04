using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;

namespace InternetBanking.Services
{
    public interface IPaymentScheduleService
    {
        Task<IntervalType> FindTypeByTypesEnum(IntervalTypesEnum enumItem);
        Task<IntervalType> FindIntervalTypeByIntervalCode(int intervalCode);
        Task<IntervalType> FindIntervalTypeById(int intervalTypeId);

        PaymentSchedule CreatePaymentSchedule(Template template, IntervalType interval, DateTime dateStart,
            string dateEnd);

        Task<bool> IsPaymentScheduleExist(Template template);
        Task<PaymentSchedule> FindPaymentSheduleByTemplate(Template template);
       
        void DeletePaymentSchedule(int paymentScheduleId);
        Task<PaymentSchedule> FindPaymentScheduleById(int paymentScheduleId);
        void CreateTransfers();
        void CheckTime(Object obj);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.IO.Pipelines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;

namespace InternetBanking.Services
{
    public class PaymentScheduleService : IPaymentScheduleService
    {
        private readonly ApplicationContext context;
        
        private readonly IServiceProvider serviceProvider;
       
        private readonly TimeSpan TimeOfPaymentCreation;

        
      
        public PaymentScheduleService(ApplicationContext context, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            TimeOfPaymentCreation = GetTimeOfPaymentCreation(configuration);


        }

        public async Task<IntervalType> FindTypeByTypesEnum(IntervalTypesEnum enumItem)
        {
            IntervalType intervalType = await context.IntervalTypes.FirstOrDefaultAsync(t => t.IntervalName == enumItem.ToString());
            return intervalType;
        }

        public async Task<IntervalType> FindIntervalTypeByIntervalCode(int intervalCode)
        {
            IntervalType intervalType =  await context.IntervalTypes.FirstOrDefaultAsync(t => t.IntervalCode == intervalCode);
            return intervalType;
        }

        public async Task<IntervalType> FindIntervalTypeById(int intervalTypeId)
        {
            IntervalType intervalType = await context.IntervalTypes.FirstOrDefaultAsync(t => t.Id == intervalTypeId);
            return intervalType;
        }


        public PaymentSchedule CreatePaymentSchedule(Template template, IntervalType interval, DateTime dateStart, string dateEnd)
        {
            PaymentSchedule paymentSchedule =
                context.PaymentSchedules.FirstOrDefault(ps => ps.TemplateId == template.Id);
            if (paymentSchedule != null)
            {
                return null;
            }
            DateTime? finishDate = FinishDateParse(dateEnd);
            PaymentSchedule schedule = new PaymentSchedule
            {
                Template = template,
                IntervalType = interval,
                Start = dateStart,
                Finish = finishDate,
                NextPaymentDate = dateStart
            };
            context.PaymentSchedules.Add(schedule);
            context.SaveChanges();
            return schedule;
        }

        public DateTime? FinishDateParse(string dateEnd)
        {
            
            DateTime date;
            bool parseResult = DateTime.TryParse(dateEnd, out date);
            if (parseResult)
            {
                return date;
            }
            else
            {
                return null;
            }
           
            
        }

        public async Task<bool> IsPaymentScheduleExist(Template template)
        {
            return await context.PaymentSchedules.FirstOrDefaultAsync(s => s.TemplateId == template.Id) != null;
        }

        public async Task<PaymentSchedule> FindPaymentSheduleByTemplate(Template template)
        {
            PaymentSchedule schedule =
                await context.PaymentSchedules.Include(s=>s.IntervalType).Include(s=>s.Template).FirstOrDefaultAsync(s => s.TemplateId == template.Id);
            return schedule;
        }
        
        public  void DeletePaymentSchedule(int paymentScheduleId)
        {
            PaymentSchedule schedule = context.PaymentSchedules.FirstOrDefault(p => p.Id == paymentScheduleId);
            context.Entry(schedule).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public async Task<PaymentSchedule> FindPaymentScheduleById(int paymentScheduleId)
        {
            PaymentSchedule schedule =
                await context.PaymentSchedules.Include(s=>s.IntervalType).Include(s=>s.Template).FirstOrDefaultAsync(p => p.Id == paymentScheduleId);
            return schedule;
        }


       

        public  void CheckTime(Object obj)
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            //Устанавливаем интервал времени в котором должны совершиться платежи 
            TimeSpan interval = new TimeSpan(0, 0,0, 20);    
            TimeSpan finishTime = TimeOfPaymentCreation.Add(interval);
            if (time > TimeOfPaymentCreation && time <= finishTime)
            {
               
                CreateTransfers();
            }
        }

        public async void CreateTransfers()
        {
            //Нельзя использовать один экземпляр ApplicationContext в разных потоках
            ApplicationContext db = serviceProvider.GetService<ApplicationContext>();

            List<PaymentSchedule> schedules = await db.PaymentSchedules.Include(s=>s.IntervalType)
                    .Include(ps => ps.Template).ThenInclude(t => t.Type)
                    .Include(ps => ps.Template).ThenInclude(t=>t.AccountSender)
                    .Include(ps=>ps.Template).ThenInclude(t=>t.AccountReceiver)
                    .Where(s => s.NextPaymentDate.Date == DateTime.Now.Date).ToListAsync();
            if (IsDayFerival())
            {
                foreach (PaymentSchedule ps in schedules)
                {
                   await CreateTransferFromTemplate(ps.Template);
                }
                UpdateNextPaymentDateRange(schedules);

                //Выполнение отложенных платежей
                await CreateDefferedTransfers();
            }
            else
            {
                await AddDeferredPayments(schedules);
            }

            
        }

        private async Task  CreateTransferFromTemplate(Template template)
        {
            //Нельзя использовать один экземпляр ApplicationContext в разных потоках
            var transferService = serviceProvider.GetService<ITransferService>();
            //var accountService = serviceProvider.GetService<IAccountService>();
            //if( await accountService.IsBalanceEnough(template.AccountSender.Id, template.Amount))
            if (template.Type.IsEqual(TypeOfTransfersEnum.InnerTransfer))
            {
                InnerTransfer transfer = await transferService.CreateInnerTransfer(template.AccountSender, template.AccountReceiver, template.Amount,
                    template.Comment, null, null, null);
                transferService.AddTransfer(transfer);
            }
            else if (template.Type.IsEqual(TypeOfTransfersEnum.InterBankTransfer))
            {
                InnerTransfer transfer = await
                    transferService.CreateInnerTransfer(template.AccountSender, template.AccountReceiver, template.Amount, template.Comment, null, null, null);
                transferService.AddTransfer(transfer);
                InterBankTransferViewModel model = new InterBankTransferViewModel
                {
                    Transfer = new InnerTransferViewModel
                    {
                        ReceiverAccountNumber = template.AccountNumber
                    },
                    ReciverName = template.ReciverName,
                    BankId = template.BankId,
                    PaymentCodeId = template.PaymentCodeId
                };
                transferService.CreateInterBankTransfer(model, transfer);
            }
        }
        public void UpdateNextPaymentDateRange(List<PaymentSchedule> schedules)
        {
            //Нельзя использовать один экземпляр ApplicationContext в разных потоках
            ApplicationContext db = serviceProvider.GetService<ApplicationContext>();

            foreach (PaymentSchedule schedule in schedules)
            {
                DateTime newDate = new DateTime();

                switch (schedule.IntervalType.IntervalCode)
                {
                    case (int)IntervalTypesEnum.OnceADay: newDate = schedule.NextPaymentDate.AddDays(1); break;
                    case (int)IntervalTypesEnum.OnceAWeek: newDate = schedule.NextPaymentDate.AddDays(7); break;
                    case (int)IntervalTypesEnum.OnceInTwoWeeks: newDate = schedule.NextPaymentDate.AddDays(14); break;
                    case (int)IntervalTypesEnum.OnceAMonth: newDate = schedule.NextPaymentDate.AddMonths(1); break;
                    case (int)IntervalTypesEnum.OnceAQuarter: newDate = schedule.NextPaymentDate.AddMonths(3); break;
                    case (int)IntervalTypesEnum.OnceAHalfYear: newDate = schedule.NextPaymentDate.AddMonths(6); break;
                    case (int)IntervalTypesEnum.OnceAYear: newDate = schedule.NextPaymentDate.AddYears(1); break;
                }

                //Если дата следующего платежа больше даты окончания - то она остается той же
                if (schedule.Finish != null && newDate > schedule.Finish)
                {
                    continue;
                }

                schedule.NextPaymentDate = newDate;
            }

            db.PaymentSchedules.UpdateRange(schedules);
            db.SaveChanges();
        }

        private bool IsDayFerival()
        {
            
            //Проверка дней недели
            DayOfWeek today = DateTime.Now.DayOfWeek;
            bool result;
            switch (today)
            {
                case DayOfWeek.Saturday:
                    result = false; break;
                case DayOfWeek.Sunday: result = false; break;
                    default: result = true; break;
            }
            //проверка на праздники
            DateTime todayDate = DateTime.Now.Date;
            List<DateTime> holidays = GetHolidays();
            
            foreach (var holiday in holidays)
            {
                if (todayDate.Date == holiday.Date)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private async Task AddDeferredPayments(List<PaymentSchedule> schedules)
        {
            //Нельзя использовать один экземпляр ApplicationContext в разных потоках
            ApplicationContext db = serviceProvider.GetService<ApplicationContext>();
            foreach (var paymentSchedule in schedules)
            {
                paymentSchedule.DeferredPaymentCounter++;
                
            }
            UpdateNextPaymentDateRange(schedules);
            db.PaymentSchedules.UpdateRange(schedules);
            await db.SaveChangesAsync();
        }

        private async Task CreateDefferedTransfers()
        {
            ApplicationContext db = serviceProvider.GetService<ApplicationContext>();
            List<PaymentSchedule> deferredPayments = db.PaymentSchedules.Include(s => s.IntervalType)
                .Include(ps => ps.Template).ThenInclude(t => t.Type)
                .Include(ps => ps.Template).ThenInclude(t => t.AccountSender)
                .Include(ps => ps.Template).ThenInclude(t => t.AccountReceiver)
                .Where(s => s.DeferredPaymentCounter > 0).ToList();
            foreach (var deferredPayment in deferredPayments)
            {
                for (int i = 0; i < deferredPayment.DeferredPaymentCounter; i++)
                {
                   await CreateTransferFromTemplate(deferredPayment.Template);
                }
                deferredPayment.DeferredPaymentCounter = 0;

            }

            db.PaymentSchedules.UpdateRange(deferredPayments);
            db.SaveChanges();
        }

        private List<DateTime> GetHolidays()
        {
            try
            {
                string datesJson = File.ReadAllText(@"ListOfHolidays.json");
                List<DateTime> holidays = JsonConvert.DeserializeObject<List<DateTime>>(datesJson);
                int currentYear = DateTime.Now.Year;
                for (int i = 0; i < holidays.Count; i++)
                {
                    holidays[i] = holidays[i].AddYears(currentYear - holidays[i].Year);
                }
                return holidays;
            }
            catch (System.IO.FileNotFoundException)
            {
                throw;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public TimeSpan GetTimeOfPaymentCreation(IConfiguration configuration)
        {
            return TimeSpan.Parse(configuration["TimeOfPaymentCreation"]);
        }
    }
}

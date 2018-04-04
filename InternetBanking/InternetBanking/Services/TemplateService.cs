using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Models.SelectTable;
using InternetBanking.ViewModels;
using InternetBanking.ViewModels.Enums;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ApplicationContext context;

        public TemplateService(ApplicationContext context)
        {
            this.context = context;
        }

        public Template SaveTemplate(InnerTransfer transfer, User user)
        {
            UserInfo userInfo = context.UserInfo.FirstOrDefault(u => u.UserId == user.Id);
            EmployeeInfo employee =
                context.EmployeeInfos.Include(c => c.Company).FirstOrDefault(e => e.UserId == user.Id);
            TypeOfTransfer type =
                context.TypeOfTransfers.FirstOrDefault(n => n.IsEqual(TypeOfTransfersEnum.InnerTransfer));
            Template template = new Template
            {
                AccountSenderId =  transfer.AccountSenderId,
                AccountReceiverId = transfer.AccountReceiverId,
                Amount =  transfer.Amount,
                Comment = transfer.Comment,
                Type = type
            };
            AddUserIdTemplate(template, userInfo, employee);

            context.Templates.Add(template);
            context.SaveChanges();
            return template;
        }

        public Template CreateTemplateInnerTransfer(Account accountSender, Account accountRecevir, decimal amount, string comment, User user)
        {
            UserInfo userInfo = context.UserInfo.FirstOrDefault(u => u.UserId == user.Id);
            EmployeeInfo employee =
                context.EmployeeInfos.Include(c => c.Company).FirstOrDefault(e => e.UserId == user.Id);
            TypeOfTransfer type = context.TypeOfTransfers.FirstOrDefault(n => n.IsEqual(TypeOfTransfersEnum.InnerTransfer));
            Template template = new Template
            {
                AccountSenderId = accountSender.Id,
                AccountReceiverId = accountRecevir.Id,
                Amount = amount,
                Comment = comment,
                Type = type
            };
            AddUserIdTemplate(template, userInfo, employee);

            context.Templates.Add(template);
            context.SaveChanges();
            return template;
        }
        public Template CreateTemplateInterTransfer(Template template, InterBankTransferViewModel model)
        {
            TypeOfTransfer type = context.TypeOfTransfers.FirstOrDefault(n => n.IsEqual(TypeOfTransfersEnum.InterBankTransfer));
            template.BankId = model.BankId;
            template.PaymentCodeId = model.PaymentCodeId;
            template.AccountNumber = model.Transfer.ReceiverAccountNumber;
            template.ReciverName = model.ReciverName;
            template.Type = type;
            
           context.Templates.Update(template);
            context.SaveChanges();
            return template;
        }


        public Template SaveTemplateInterTransfer(InterBankTransfer transfer , User user)
        {
            UserInfo userInfo = context.UserInfo.FirstOrDefault(u => u.UserId == user.Id);
            EmployeeInfo employee =
                context.EmployeeInfos.Include(c => c.Company).FirstOrDefault(e => e.UserId == user.Id);
            TypeOfTransfer type = context.TypeOfTransfers.FirstOrDefault(n => n.IsEqual(TypeOfTransfersEnum.InterBankTransfer));
            Template template = new Template
            {
                AccountSenderId = transfer.Transfer.AccountSenderId,
                AccountReceiverId = transfer.Transfer.AccountReceiverId,
                Amount = transfer.Transfer.Amount,
                Comment = transfer.Transfer.Comment,
                BankId = transfer.BankId,
                PaymentCodeId = transfer.PaymentCodeId,
                AccountNumber = transfer.AccountNumber,
                ReciverName = transfer.ReciverName,
                Type=type

            };

            AddUserIdTemplate(template, userInfo, employee);

            context.Templates.Add(template);
            context.SaveChanges();
            return template;
        }

        public Template FindTemplateById(int? templateId)
        {
            Template template = GetTemplats().FirstOrDefault(t=> t.Id== templateId);
            return template;
        }

        public Template AddTemplateNameDisc(Template template, TemplateViewModel model)
        {
            template.TempalteName = model.TempalteName;
            template.TemplateDiscription = model.TemplateDiscription;

            context.Templates.Update(template);
            context.SaveChanges();

            return template;
        }

        public IQueryable<Template> GetTempaleList(User user)
        {
            UserInfo userInfo = context.UserInfo.FirstOrDefault(u => u.UserId == user.Id);
            EmployeeInfo employee =
                context.EmployeeInfos.Include(c => c.Company).FirstOrDefault(e => e.UserId == user.Id);
            if (userInfo != null)
            {
                IQueryable<Template> tempaleList = GetTemplats().Where(u => u.UserInfoId == userInfo.Id);
                return tempaleList;
            }
            else if (employee != null)
            {
                IQueryable<Template> tempaleList = GetTemplats().Where(u => u.CompanyId == employee.Company.Id);
                return tempaleList;
            }
            else
            {
                IQueryable<Template> tempaleList = GetTemplats();
                return tempaleList;
            }

        }

        private void AddUserIdTemplate( Template template, UserInfo userInfo, EmployeeInfo employee)
        {
            if (userInfo != null)
            {
                template.UserInfoId = userInfo.Id;
            }

            if (employee != null)
            {
                template.CompanyId = employee.Company.Id;
            }
        }

        private IQueryable<Template> GetTemplats()
        {
            IQueryable<Template> templats = context.Templates.Include(ui=>ui.UserInfo).Include(c=>c.Company).Include(a => a.AccountReceiver).Include(a => a.AccountSender).Include(b=>b.Bank).Include(pc => pc.PaymentСode).Include(t=>t.Type);
            return templats;
        }
    }
}

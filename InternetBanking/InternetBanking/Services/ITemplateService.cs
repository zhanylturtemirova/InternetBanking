using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;

namespace InternetBanking.Services
{
    public interface ITemplateService
    {
        Template SaveTemplate(InnerTransfer transfer, User user);
        Template SaveTemplateInterTransfer(InterBankTransfer transfer, User user);
        Template FindTemplateById(int? templateId);
        Template AddTemplateNameDisc(Template template, TemplateViewModel model);
        IQueryable<Template> GetTempaleList(User user);

        Template CreateTemplateInnerTransfer(Account accountSender, Account accountRecevir, decimal amount,
            string comment, User user);

        Template CreateTemplateInterTransfer(Template template, InterBankTransferViewModel model);
    }
}

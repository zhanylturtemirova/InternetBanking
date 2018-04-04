using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Xunit;
using Moq;
using InternetBanking.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using EncryptStringSample;
using InternetBanking.Services;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Math.EC;

namespace InternetBanking.Tests
{
    public static class TestServicesProvider
    {
        public static ApplicationContext GetContext()
        {
            DirectoryInfo path = new DirectoryInfo(@"..\..\..");
            //IConfigurationRoot Configuration = new ConfigurationBuilder()
            IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(path.FullName)
            .AddJsonFile("appsettings.json")
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationContext(optionsBuilder.Options);
        }

      /*  private static UserManager<User> GetUserManager()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            UserManager<User> userManager = new AspNetUserManager<User>();
        }*/

        public static ModelTestData GetModelTestData()
        {
            ApplicationContext context = GetContext();
            ModelTestData modelTestData = new ModelTestData(context);
            return modelTestData;
        }

        public static IAccountService GetAccountService()
        {
            ApplicationContext context = GetContext();
            IAccountService service = new AccountService(context, new SelectListService(context), new BankService(context));
            return service;
        }

        
        public static IBankService GetBankService()
        {
            ApplicationContext context = GetContext();
            IBankService service = new BankService(context);
            return service;
        }
        public static ICompanyService GetCompanyService()
        {
            ApplicationContext context = GetContext();
            ICompanyService service = new CompanyService(context, new HostingEnvironment(), GetFileUploadService());
            return service;
        }
        public static FileUploadService GetFileUploadService()
        {
            FileUploadService service = new FileUploadService();
            return service;
        }
        public static ICurrencyService GetCurrencyService()
        {
            ApplicationContext context = GetContext();
            ICurrencyService service = new CurrencyService(context);
            return service;
        }
        public static IDocumentFormatService GetDocumentFormatService()
        {
            IDocumentFormatService service = new DocumentFormatService(new HostingEnvironment());
            return service;
        }
        public static IEmailService GetEmailService()
        {
            IEmailService service = new EmailService();
            return service;
        }

       
        public static IExchangeRateService GetExchangeRateService()
        {
            ApplicationContext context = GetContext();
            IExchangeRateService service = new ExchangeRateService(context, GetAccountService());
            return service;
        }
        public static IDocumentService GetDocumentService()
        {
            ApplicationContext context = GetContext();
            IDocumentService service = new DocumentService(context, new HostingEnvironment(), GetFileUploadService());
            return service;
        }
        public static GeneratePasswordService GetGeneratePasswordService()
        {
            GeneratePasswordService service = new GeneratePasswordService();
            return service;
        }

        public static ILimitService GetLimitService()
        {
            ApplicationContext context = GetContext();
            ILimitService service = new LimitService(context);
            return service;
        }

        public static IPaymentCodeService GetPaymentCodeService()
        {
            ApplicationContext context = GetContext();
            IPaymentCodeService service = new PaymentCodeService(context);
            return service;
        }

        /*public static IPaymentScheduleService GetPaymentScheduleService()
        {
            ApplicationContext context = GetContext();
            IPaymentScheduleService service = new PaymentScheduleService(context, new ServiceContainer(),);
            return service;
        }*/

        public static ICreatePDFandLoad GetPdfCreateAndLoadService()
        {
            ICreatePDFandLoad service = new PdfCreateAndLoadService(new HostingEnvironment());
            return service;
        }


        public static ISelectListService GetSelectListService()
        {
            ApplicationContext context = GetContext();
            ISelectListService service = new SelectListService(context);
            return service;
        }


        public static ITemplateService GetTemplateService()
        {
            ApplicationContext context = GetContext();
            ITemplateService service = new TemplateService(context);
            return service;
        }


        public static ITransactionService GetTransactionService()
        {
            ApplicationContext context = GetContext();
            ITransactionService service = new TransactionService(context);
            return service;
        }

        /*public static ITransferService GetTransferService()
        {
            ApplicationContext context = GetContext();
            ITransferService service = new TransferService(context, GetTransactionService(), new UserService(), GetSelectListService(), GetAccountService() );
            return service;
        }*/

        /*public static IValidationService GetValidationService()
        {
            ApplicationContext context = GetContext();
            IValidationService service = new ValidationService();
            return service;
        }*/


        public static IXmlService GetXmlService()
        {
            
            IXmlService service = new XmlService();
            return service;
        }

    }
}

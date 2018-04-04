using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace InternetBanking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISelectListService, SelectListService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IExchangeRateService, ExchangeRateService>();

            services.AddTransient<IGeneratePassword, GeneratePasswordService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPaymentCodeService, PaymentCodeService>(); 
            services.AddTransient<ILimitService, LimitService>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IDocumentFormatService, DocumentFormatService>();
            services.AddTransient<FileUploadService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<NoPagingService>();
            services.AddTransient<PagingService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<ITransferService, TransferService>();
            AddPagingService<IHomePagingService, PagingService, NoPagingService>(services, "PagingModeHome");
            services.AddTransient<IXmlService, XmlService>();
            services.AddTransient<IPaymentScheduleService, PaymentScheduleService>();
            services.AddTransient<ICreatePDFandLoad, PdfCreateAndLoadService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().AddDataAnnotationsLocalization().AddViewLocalization();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        }
        private void AddPagingService<TI, TP, TN>(IServiceCollection services, string key)
            where TI : IHomePagingService
            where TP : PagingService
            where TN : NoPagingService
        {
            PagingMode pagingMode = GetPagingMode(key);

            switch (pagingMode)
            {
                case PagingMode.NoPaging:
                    services.AddTransient(typeof(TI), typeof(TN));
                    break;
                default:
                    services.AddTransient(typeof(TI), typeof(TP));
                    break;
            }
        }
        private PagingMode GetPagingMode(string key)
        {
            string pagingModeSetting = Configuration[key];
            return Enum.Parse<PagingMode>(pagingModeSetting);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            var supportedCultures = new[]
                        {
                
                new CultureInfo("ru")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });


            app.UseAuthentication();

            app.UseDeveloperExceptionPage();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InternetBanking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            IPaymentScheduleService scheduleService = null;
           
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationContext>();
                    scheduleService = services.GetRequiredService<IPaymentScheduleService>();
                    var _userManager = services.GetService<UserManager<User>>();
                    ModelData.FillData(context, _userManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error while initializing model data");
                }
            }
            //Запускаем сервис совершения платежей по расписанию
            TimerCallback timerCallback = new TimerCallback(scheduleService.CheckTime);
            Timer timer = new Timer(timerCallback, null, 0, 20000);
           
                webHost.Run();
            
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

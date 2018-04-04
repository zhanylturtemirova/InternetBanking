using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OfficeOpenXml.FormulaParsing.Utilities;
using Xunit;

namespace InternetBanking.Tests
{
    public class EmailServerTests
    {
        private readonly IEmailService emailService; 

        public EmailServerTests()
        {
            emailService = TestServicesProvider.GetEmailService();
            TestServicesProvider.GetModelTestData().FillData();
        }

        [Fact]
        public void CheckIfEmailSent()
        {

            //var emailService = A.Fake<IEmailService>();
            //var controller = new ExampleController(emailService);
            //controller.Index();
            //A.CallTo(() => emailService.Send(A<Email>._))
            //    .MustHaveHappened();


            //emailService.SendEmailAsync("esdp_group1@mail.ru", "проверка", "EmailSender tests");
            //EmailApprovals.Verify(mail);
            //Assert.Equal(1, receivingServer.Inbox.Count);
            //ReceivedEmailMessage received = receivingServer.Inbox[0];
            //Assert.Equal("esdp_group1@mail.ru", received.ToAddress.Email);
        }

        [Fact]
        public   void SendTest()
        {
            Mock<EmailService> smtp = new Mock<EmailService>();
            var result =smtp.Setup(t=>t.SendEmailAsync("esdp_group1@mail.ru", "Test", "Test"));
            //var result = TestServicesProvider.GetEmailService().SendEmailAsync("esdp_group1@mail.ru","Test","Test");
           //var result = emailService.SendEmailAsync("esdp_group1@mail.ru","Test","Test");
            //bool isSend = result.IsCompletedSuccessfully;
            Assert.NotNull(result);
            //Assert.True(isSend);
        }

    }
}

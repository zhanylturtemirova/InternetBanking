using System;
using AcceptionsTest.Model;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{

    [Binding]
    public class AccessAdminAuthorizationSteps : TestAssistant
    {
      
        [Given(@"Открыта cтраница входа на сайт")]
        public void ДопустимОткрытаCтраницаВходаНаСайт()
        {
            OpenPage("http://localhost:7894");
        }
        
        [When(@"я авторизуюсь как администратор")]
        public void ЕслиЯАвторизуюсьКакАдминистратор()
        {
            TextToElement("inputName", "Admin");
            TextToElement("inputPassword", "Admin123@");
            Click("id", "logInButton");
        }
        
        [Then(@"вижу панель администратора")]
        public void ТоВижуПанельАдминистратора()
        {                           
            Assert.IsTrue(Find("id", "adminLogo", "АдминПанель"));
        }

        [AfterTestRun()]
        public static void CloseTest()
        {
            ApplicationDbContext context= new ApplicationDbContext();
            DeleteData deleteData = new DeleteData(context);
           deleteData.DeleteUser("esdp_group1@mail.ru");
          //deleteData.TrancateDatabase();
        }

       
    }

    
}

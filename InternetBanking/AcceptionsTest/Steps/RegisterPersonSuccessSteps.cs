using System;
using AcceptionsTest.Model;
using AcceptionsTest.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterPersonSuccessSteps : TestAssistant
    {
        
        [Given(@"Открыта cтраница регистрации физ лица")]
        public void ДопустимОткрытаCтраницаРегистрацииФизЛица()
        {
            OpenPage("http://localhost:7894");
            TextToElement("inputName", "Admin");
            TextToElement("inputPassword", "Admin123@");
            Click("id", "logInButton");
            Click("id", "LeftNav");
            WaitingForElement();
            SummaryDisplayed("RegisterPerson");
            Click("id", "RegisterPerson");
        }
        
        [When(@"заполняю все поля в форме регистрации правильно")]
        public void ЕслиЗаполняюВсеПоляВФормеРегистрацииПравильно()
        {
            RegisterPersonViewModel model = ModelData.RegPersonSuccess();

            TextToElement("FName", model.UserInfo.FirstName);
            TextToElement("SName", model.UserInfo.SecondName);
            TextToElement("TName", model.UserInfo.MiddleName);
            Click("id", "PassportTODselect");
            Click("xpath", "//option[text()='Паспорт']");
            TextToElement("PassportSeries", model.PassportInfo.Series);
            TextToElement("PassportDate", model.PassportInfo.DateofExtradition);
            TextToElement("PassportValidaty", model.PassportInfo.Validaty);
            TextToElement("PassportNumber", model.PassportInfo.Number);
            TextToElement("PassportIssedBy", model.PassportInfo.IssuedBy);
            TextToElement("Inn", model.UserInfo.Inn);
            Click("id","Male");
            TextToElement("BirthDay", model.UserInfo.BirthDay);
            TextToElement("MobilePhone", model.ContactInfo.MobilePhone);
            TextToElement("ContactEmail", model.UserInfo.Email);
            Click("id", "SendClick");
        }
        
        [When(@"создаю счет физ лицу")]
        public void ЕслиСоздаюСчетФизЛицу()
        {
            Click("xpath", "//span[text()='Выберите валюту'] ");
            Click("xpath", "//option[text()='SOM']");
            Click("xpath", "//span[text()='Выберите лимит'] ");
            Click("xpath", "//option[text()='NoLimit']");
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу со счетами физ лица")]
        public void ТоВижуСтраницуСоСчетамиФизЛица()
        {
            Assert.IsTrue(Find("id", "AccountNumber", "123"));            
        }


        
        
    }
}

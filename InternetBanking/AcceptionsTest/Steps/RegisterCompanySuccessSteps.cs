using System;
using AcceptionsTest.Model;
using AcceptionsTest.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterCompanySuccessSteps : TestAssistant
    {
        [Given(@"Открыта cтраница регистрации юр лица")]
        public void ДопустимОткрытаCтраницаРегистрацииЮрЛица()
        {
            OpenPage("http://localhost:7894");
            TextToElement("inputName", "Admin");
            TextToElement("inputPassword", "Admin123@");
            Click("id", "logInButton");
            Click("id", "LeftNav"); 
            SummaryDisplayed("AddCompany");
            Click("id", "AddCompany");
        }
        
        [When(@"заполняю все поля в форме регистрации юр лица правильно")]
        public void ЕслиЗаполняюВсеПоляВФормеРегистрацииЮрЛицаПравильно()
        {
            AddCompanyViewModel model = ModelData.RegCompanySuccess();
            TextToElement("CompanyName", model.NameCompany);
            Click("id", "LegalForm");
            Click("xpath", "//option[text()='ОАО']");
            Click("id", "PropertyType");
            Click("xpath", "//option[text()='Частная']");
            TextToElement("Inn", model.InnCompany);
            TextToElement("Okpo", model.OkpoCompany);
            TextToElement("SocialFundNumber", model.RegistrationNumberSocialFund);
            TextToElement("RegAuthority", model.RegistrationAuthority);
            TextToElement("DORministryJustice", model.DateOfRegistrationMinistryJustice);
            TextToElement("IssuedBy", model.IssuedBy);
            TextToElement("DateofInitialReg", model.DateOfInitialRegistration);
            Click("id", "TaxInspection");
            Click("xpath", "//option[text()='Ленинский']");
            Click("id", "Residency");
            Click("xpath", "//option[text()='Резидент']");
            TextToElement("MobilePhone", model.ContactInfo.MobilePhone);

            Click("id", "SendClick");
        }

        [When(@"создаю сотрудника")]
        public void ЕслиСоздаюСотрудника()
        {
            RegisterEmployeeViewModel model = ModelData.RegEmployeeSuccess();
            TextToElement("FName", model.FirstName);
            TextToElement("SName", model.SecondName);
            TextToElement("TName", model.MiddleName);
            TextToElement("Position", model.Position);
            TextToElement("Email", model.Email);
            Click("id", "SendClick");
        }
        [When(@"создаю счет юр лицу")]
        public void ЕслиСоздаюСчетЮрЛицу()
        {
            Click("id", "Accounts");
            Click("id", "AddCompanyAccount");
            Click("id", "CurrencySelect");
            Click("xpath", "//option[text()='SOM']");
            Click("xpath", "//span[text()='Выберите лимит'] ");
            Click("xpath", "//option[text()='NoLimit']");
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу с информацией о компании")]
        public void ТоВижуСтраницуСИнформациейОКомпании()
        {
            Assert.IsTrue(Find("id", "NameCompany", "Samsung"));
        }
    }
}

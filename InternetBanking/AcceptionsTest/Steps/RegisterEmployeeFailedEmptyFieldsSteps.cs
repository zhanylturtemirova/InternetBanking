using System;
using AcceptionsTest.Model;
using AcceptionsTest.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterEmployeeFailedEmptyFieldsSteps : TestAssistant
    {
        [Given(@"Открыта cтраница регистрации сотрудника")]
        public void ДопустимОткрытаCтраницаРегистрацииСотрудника()
        {
            OpenPage("http://localhost:7894");
            TextToElement("inputName", "Admin");
            TextToElement("inputPassword", "Admin123@");
            Click("id", "logInButton");
            Click("id", "LeftNav");
            SummaryDisplayed("AddCompany");
            Click("id", "AddCompany");
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
        
        [When(@"не заполняю поля в форме сотрудника")]
        public void ЕслиНеЗаполняюПоляВФормеСотрудника()
        {
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу с валидацией формы сотрудника")]
        public void ТоВижуСтраницуСВалидациейФормыСотрудника()
        {
            Assert.IsTrue(Find("id", "FNameValid", "Поле"));
            Assert.IsTrue(Find("id", "SNameValid", "Поле"));
            Assert.IsTrue(Find("id", "TNameValid", "Поле"));
            Assert.IsTrue(Find("id", "PositionValid", "Поле"));
            Assert.IsTrue(Find("id", "EmailValid", "Поле"));
        }
    }
}

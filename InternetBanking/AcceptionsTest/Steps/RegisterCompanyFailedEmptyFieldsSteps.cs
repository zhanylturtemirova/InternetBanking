using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterCompanyFailedEmptyFieldsSteps : TestAssistant
    {
        [When(@"не заполняю все поля в форме регистрации юр лица")]
        public void ЕслиНеЗаполняюВсеПоляВФормеРегистрацииЮрЛица()
        {
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу регистрации юр лица с валидацией о пустых полях\.")]
        public void ТоВижуСтраницуРегистрацииЮрЛицаСВалидациейОПустыхПолях_()
        {
            Assert.IsTrue(Find("id", "NameCompanyValid", "Поле"));
            Assert.IsTrue(Find("id", "LegalFormValid", "Выберите"));
            Assert.IsTrue(Find("id", "PropertyTypeValid", "Выберите"));
            Assert.IsTrue(Find("id", "InnValid", "Поле"));
            Assert.IsTrue(Find("id", "OkpoValid", "Поле"));
            Assert.IsTrue(Find("id", "SocialFundNumberValid", "Поле"));
            Assert.IsTrue(Find("id", "RegAuthorityValid", "Поле"));
            Assert.IsTrue(Find("id", "DORministryJusticeValid", "Поле"));
            Assert.IsTrue(Find("id", "IssuedByValid", "Поле"));
            Assert.IsTrue(Find("id", "DateofInitialRegValid", "Поле"));
            Assert.IsTrue(Find("id", "TaxInspectionValid", "Выберите"));
            Assert.IsTrue(Find("id", "ResidencyValid", "Выберите"));
            Assert.IsTrue(Find("id", "MobilePhoneValid", "Поле"));
        }
    }
}

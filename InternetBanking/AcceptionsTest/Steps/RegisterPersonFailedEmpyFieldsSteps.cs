using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterPersonFailedEmpyFieldsSteps : TestAssistant
    {
        [When(@"не заполняю все поля в форме регистрации")]
        public void ЕслиНеЗаполняюВсеПоляВФормеРегистрации()
        {
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу регистрации физ лица с валидацией о пустых полях\.")]
        public void ТоВижуСтраницуРегистрацииФизЛицаСВалидациейОПустыхПолях_()
        {
            Assert.IsTrue(Find("id", "FNameValid", "Поле"));
            Assert.IsTrue(Find("id", "SNameValid", "Поле"));
            Assert.IsTrue(Find("id", "TNameValid", "Поле"));
            Assert.IsTrue(Find("id", "PassportSeriesValid", "Поле"));
            Assert.IsTrue(Find("id", "PassportDateValid", "Поле"));
            Assert.IsTrue(Find("id", "PassportValidatyValid", "Поле"));
            Assert.IsTrue(Find("id", "PassportNumberValid", "Поле"));
            Assert.IsTrue(Find("id", "PassportIssuedByValid", "Поле"));
            Assert.IsTrue(Find("id", "InnValid", "Поле"));
            Assert.IsTrue(Find("id", "GenderValid", "Выберите"));
            Assert.IsTrue(Find("id", "BirthDayValid", "Дата")); 
            Assert.IsTrue(Find("id", "ContactMobilePhoneValid", "Поле"));
            Assert.IsTrue(Find("id", "EmailValid", "Поле"));
        }
    }
}

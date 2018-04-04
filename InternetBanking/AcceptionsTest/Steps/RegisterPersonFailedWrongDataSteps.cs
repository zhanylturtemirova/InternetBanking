using System;
using AcceptionsTest.Model;
using AcceptionsTest.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterPersonFailedWrongDataSteps : TestAssistant
    {
        [When(@"заполняю поля в форме регистрации неправильно")]
        public void ЕслиЗаполняюПоляВФормеРегистрацииНеправильно()
        {

            TextToElement("PassportDate", "13.03.1900");
            TextToElement("PassportValidaty", "13.03.2018");
            TextToElement("Inn", "151asd");
            TextToElement("BirthDay", "13.03.1900");
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу регистрации физ лица с валидацией о неправильном вводе данных\.")]
        public void ТоВижуСтраницуРегистрацииФизЛицаСВалидациейОНеправильномВводеДанных_()
        {
            Assert.IsTrue(Find("id", "PassportDateValid", "не может быть из будущего или прошлого"));
            Assert.IsTrue(Find("id", "PassportValidatyValid", "Срок действия не может превышать 10 лет"));
            Assert.IsTrue(Find("id", "InnValid", "неверный формат ИНН"));
            Assert.IsTrue(Find("id", "BirthDayValid", "не может быть из будущего или прошлого"));
        }
    }
}

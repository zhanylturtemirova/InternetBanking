using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class RegisterCompanyWrongDataSteps : TestAssistant
    {
        [When(@"заполняю поля в форме регистрации юр лица неправильно")]
        public void ЕслиЗаполняюПоляВФормеРегистрацииЮрЛицаНеправильно()
        {
            TextToElement("DORministryJustice", "13.03.2019");
            TextToElement("DateofInitialReg", "13.03.2019");
            Click("id", "SendClick");
        }
        
        [Then(@"вижу страницу регистрации юр лица о неправильном вводе данных\.")]
        public void ТоВижуСтраницуРегистрацииЮрЛицаОНеправильномВводеДанных_()
        {
            Assert.IsTrue(Find("id", "DORministryJusticeValid", "Вы указали дату из будущего!"));          
            Assert.IsTrue(Find("id", "DateofInitialRegValid", "Вы указали дату из будущего!"));
        }
    }
}

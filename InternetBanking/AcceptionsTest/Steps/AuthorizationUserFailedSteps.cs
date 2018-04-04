using System;
using AcceptionsTest.Model;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AcceptionsTest.Steps
{
    [Binding]
    public class AuthorizationUserFailedSteps : TestAssistant
    {
        [Given(@"Открыта страница входа на сайт")]
        public void ДопустимОткрытаСтраницаВходаНаСайт()
        {            
            OpenPage("http://localhost:7894");
        }
        
        [When(@"я авторизуюсь с неправильным паролем")]
        public void ЕслиЯАвторизуюсьСНеправильнымПаролем()
        {
            TextToElement("inputName", "Admin");
            TextToElement("inputPassword", "Amin123@");
            Click("id", "logInButton");
        }
        
        [Then(@"на странице выдается сообщение об оставшихся попытках")]
        public void ТоНаСтраницеВыдаетсяСообщениеОбОставшихсяПопытках()
        {
            Assert.IsTrue(Find("id", "passwordIncorrect", "Неверный пароль"));
        }

       
    }
}

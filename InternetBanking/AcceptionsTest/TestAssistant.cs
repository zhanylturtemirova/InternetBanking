using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace AcceptionsTest
{
    [Binding]
    public class TestAssistant
    {
        private IWebDriver driver = WebDriverManager.GetDriver();

       

        public void OpenPage(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public bool Find(string typeSelector, string nameSelector, string text)
        {
            bool result = false;
            switch (typeSelector)
            {
                case "css":
                    IWebElement searchSelectorCss = driver.FindElement(By.CssSelector(nameSelector));
                    if (searchSelectorCss.Text.Contains(text))
                    {
                        result = true;
                    }
                    break;
                case "id":
                    IWebElement searchButtonId = driver.FindElement(By.Id(nameSelector));
                    if (searchButtonId.Text.Contains(text))
                    {
                        result = true;
                    }
                    break;
                case "class":
                    IWebElement searchButtonClass = driver.FindElement(By.ClassName(nameSelector));
                    if (searchButtonClass.Text.Contains(text))
                    {
                        result = true;
                    }
                    break;
            }

            return result;
        }
        public bool SummaryDisplayed(string name)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
                var myElement = wait.Until(x => x.FindElement(By.Id(name)).Displayed);
                return myElement;
            }
            catch
            {
                return false;
            }
        }
        public void WaitingForElement()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }
        public void TextToElement(string id, string text)
        {
            IWebElement searchElement = driver.FindElement(By.Id(id));
            searchElement.SendKeys(text);
        }

        public void Click(string typeSelector, string name)
        {
            switch (typeSelector)
            {
                case "css":

                    IWebElement searchButtonCss = driver.FindElement(By.CssSelector(name));                  
                    searchButtonCss.Click();
                    break;
                case "id":
                    IWebElement searchButtonId = driver.FindElement(By.Id(name));
                    searchButtonId.Click();
                    break;
                case "class":
                    IWebElement searchButtonClass = driver.FindElement(By.ClassName(name));
                    searchButtonClass.Click();
                    break;
                case "xpath":
                    IWebElement searchElemName = driver.FindElement(By.XPath(name));
                    searchElemName.Click();
                    break;
            }
        }
        
    }
}

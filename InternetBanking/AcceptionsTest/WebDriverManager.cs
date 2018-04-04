using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace AcceptionsTest
{
    public class WebDriverManager
    {
        private static IWebDriver driver;

        [BeforeFeature()]
        public static void SetUp()
        {
            driver = WebDriverManager.GetDriver();
        }

        [AfterScenario()]
        public static void TreadDown()
        {
            driver.Quit();
        }

        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                driver = new ChromeDriver();
            }
            return driver;
        }
    }
}

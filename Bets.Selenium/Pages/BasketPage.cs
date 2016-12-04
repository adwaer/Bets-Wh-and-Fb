using System;
using System.Text;
using Bets.Domain.PageElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Bets.Selenium.Pages
{
    public abstract class BasketPage
    {
        protected virtual void Setup(IWebDriver webDriver, string url)
        {
            webDriver
                .Navigate()
                .GoToUrl(url);
        }

        protected IWebDriver GetNewDriver(string driverName)
        {
            RemoteWebDriver driver;
            if (driverName.Equals("firefox", StringComparison.CurrentCultureIgnoreCase))
            {
                driver = new FirefoxDriver();
            }
            else if (driverName.Equals("chrome", StringComparison.CurrentCultureIgnoreCase))
            {
                driver = new ChromeDriver();
            }
            else
            {
                driver = new PhantomJSDriver();
            }

            var timeouts = driver.Manage().Timeouts();
            timeouts.ImplicitlyWait(TimeSpan.FromSeconds(20));
            timeouts.SetPageLoadTimeout(TimeSpan.FromSeconds(20));
            timeouts.SetScriptTimeout(TimeSpan.FromSeconds(20));

            return driver;
        }

        public abstract IRow[] GetRows(StringBuilder errBuilder);
    }
}

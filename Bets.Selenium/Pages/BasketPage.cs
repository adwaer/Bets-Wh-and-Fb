using System;
using System.Configuration;
using System.Text;
using System.Threading;
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
            var navigation = webDriver
                .Navigate();
            
            var isLoaded = false;
            while (!isLoaded)
            {
                try
                {
                    navigation.GoToUrl(url);
                    isLoaded = true;
                }
                catch
                {
                    // ignored
                }
            }
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
            timeouts.ImplicitlyWait(TimeSpan.FromSeconds(60));
            timeouts.SetPageLoadTimeout(TimeSpan.FromSeconds(60));
            timeouts.SetScriptTimeout(TimeSpan.FromSeconds(60));
            
            return driver;
        }

        public abstract IRow[] GetRows(StringBuilder errBuilder);
    }
}

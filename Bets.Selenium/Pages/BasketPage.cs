using System;
using System.Drawing;
using System.Text;
using Bets.Domain.PageElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

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
                    WaitForPageLoad(webDriver);
                    isLoaded = true;
                }
                catch (Exception ex)
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
                //var options = new FirefoxOptions();
                //options.
                driver = new FirefoxDriver();
            }
            else if (driverName.Equals("chrome", StringComparison.CurrentCultureIgnoreCase))
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                driver = new ChromeDriver(options);
                //driver.Manage().Window.Size
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

        IWebElement _page = null;
        private void WaitForPageLoad(IWebDriver driver)
        {
            if (_page != null)
            {
                var waitForCurrentPageToStale = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                waitForCurrentPageToStale.Until(ExpectedConditions.StalenessOf(_page));
            }

            var js = driver as IJavaScriptExecutor;
            var waitForDocumentReady = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            waitForDocumentReady.Until(wdriver => js.ExecuteScript("return document.readyState").Equals("complete"));
            //js.ExecuteScript("document.body.style.zoom='70%'");
        }
        
    }
}

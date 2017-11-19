using System;
using System.Text;
using System.Threading.Tasks;
using Bets.Domain;
using Bets.Domain.PageElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Bets.Selenium.Pages
{
    public abstract class BasketPage : IBettingPage
    {
        protected IWebDriver WebDriver;

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

        #region IBettingPage

        public Task<bool> SetTotal(StatViewModel model, decimal amount, bool more)
        {
            return Task.Run(() =>
            {
                lock (WebDriver)
                {
                    try
                    {
                        ClearBets();
                        if (more)
                        {
                            model.Game.TotalMoreElement.Click();
                        }
                        else
                        {
                            model.Game.TotalLessElement.Click();
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        public Task<bool> SetHc(StatViewModel model, decimal amount, bool more)
        {
            return Task.Run(() =>
            {
                lock (WebDriver)
                {
                    try
                    {
                        ClearBets();
                        if (more)
                        {
                            model.Game.HandicapMoreElement.Click();
                        }
                        else
                        {
                            model.Game.HandicapLessElement.Click();
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        protected abstract void ClearBets();
        #endregion


        readonly IWebElement _page = null;
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

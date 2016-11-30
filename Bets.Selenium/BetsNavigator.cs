using System;
using System.Configuration;
using System.Threading;
using Bets.Selenium.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Bets.Selenium
{
    public class BetsNavigator : IDisposable
    {

        private static object _sync = new object();
        private static bool _inited;
        private static BetsNavigator _instance;

        public static BetsNavigator Instance = LazyInitializer.EnsureInitialized(ref _instance, ref _inited, ref _sync,
            () => new BetsNavigator());


        private readonly FonbetOnlineBasketPage _fonbetOnlineBasketPage;
        private readonly WinlineOnlineBasketPage _winlineOnlineBasketPage;
        private readonly WinlineOnlineBasketPage _winlineOnlineBasketPage1;

        public BetsNavigator()
        {
            _fonbetOnlineBasketPage = new FonbetOnlineBasketPage(GetNewDriver(ConfigurationManager.AppSettings["fonbetDriver"]), ConfigurationManager.AppSettings["fonbetUrl"]);
            _winlineOnlineBasketPage = new WinlineOnlineBasketPage(GetNewDriver(ConfigurationManager.AppSettings["wlDriver"]), ConfigurationManager.AppSettings["wlUrl"]);
            _winlineOnlineBasketPage1 = new WinlineOnlineBasketPage(GetNewDriver(ConfigurationManager.AppSettings["wlDriver"]), ConfigurationManager.AppSettings["wlUrl"]);
        }

        private IWebDriver GetNewDriver(string driverName)
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

        public FonbetOnlineBasketPage FonbetPage => _fonbetOnlineBasketPage;
        public WinlineOnlineBasketPage WinlinePage => _winlineOnlineBasketPage;
        public WinlineOnlineBasketPage WinlinePage1 => _winlineOnlineBasketPage1;

        public void Dispose()
        {
            _fonbetOnlineBasketPage.WebDriver?.Close();
            _winlineOnlineBasketPage.WebDriver?.Close();
            _winlineOnlineBasketPage1.WebDriver?.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bets.Domain.PageElements;
using Bets.Services;
using OpenQA.Selenium;

namespace Bets.Selenium.Pages
{
    public sealed class WinlineOnlineBasketPage : BasketPage, IDisposable
    {
        public readonly IWebDriver WebDriver;
        public readonly IWebDriver WebDriver1;
        public readonly IJavaScriptExecutor JsHc;
        public readonly IJavaScriptExecutor JsFb;
        private bool _tabHcSwitched;
        private bool _tabTotalSwitched;

        public WinlineOnlineBasketPage()
        {
            WebDriver = GetNewDriver(ConfigurationManager.AppSettings["wlDriver"]);
            WebDriver1 = GetNewDriver(ConfigurationManager.AppSettings["wlDriver"]);
            JsHc = (IJavaScriptExecutor)WebDriver1;
            JsFb = (IJavaScriptExecutor)WebDriver;

            Setup(WebDriver, ConfigurationManager.AppSettings["wlUrl"]);
            Setup(WebDriver1, ConfigurationManager.AppSettings["wlUrl"]);
            Auth(WebDriver);
        }

        public override IRow[] GetRows(StringBuilder errBuilder)
        {
            var winlineRows = GetTotalRows(errBuilder);
            var hadicapRows = GetHadicapRows(errBuilder);

            foreach (var winlineRow in winlineRows)
            {
                var hcRow = hadicapRows.Where(r => r.Team1.Equals(winlineRow.Team1) || r.Team2.Equals(winlineRow.Team2)).ToArray();
                if (hcRow.Length == 1)
                {
                    winlineRow.HandicapElement = hcRow.First().HandicapElement;
                }
                else if (hcRow.Length > 1)
                {
                    errBuilder.AppendLine($"Дважды: {winlineRow.Team1} или {winlineRow.Team2}");
                }
            }

            return winlineRows;
        }

        private void SwitchBasketHcTab()
        {
            if (!_tabHcSwitched)
            {
                while (!_tabHcSwitched)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        JsHc.ExecuteScript(
                            "(function() { $('.sorting__item.sorting__item[title=\"Баскетбол\"]').click(); })()");
                        Thread.Sleep(2000);
                        JsHc.ExecuteScript(
                            "(function() { document.getElementsByClassName('event-tabs__link')[1].click(); })()");

                        _tabHcSwitched = true;
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
        private void SwitchBasketTab()
        {
            if (!_tabTotalSwitched)
            {
                while (!_tabTotalSwitched)
                {
                    try
                    {
                        JsFb.ExecuteScript(
                            "(function() { $('.sorting__item.sorting__item[title=\"Баскетбол\"]').click(); })()");

                        _tabTotalSwitched = true;
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        public WinlineRow[] GetTotalRows(StringBuilder errBuilder)
        {
            SwitchBasketTab();

            var results = new List<WinlineRow>();
            ReadOnlyCollection<IWebElement> tableElements = WebDriver.FindElements(By.CssSelector(".events .table .table__item"));

            Parallel.For(0, tableElements.Count, index =>
            {
                var tableElement = tableElements[index];
                try
                {
                    var teams = tableElement.FindElement(By.ClassName("statistic__team"))
                        .Text//.Replace("\r\n", string.Empty)
                        .Split(new[] { " - ", " − ", " — ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    results.Add(new WinlineRow
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams.First().Replace(" ", "")),
                        Team2 = TeamsHolder.Instance.GetTeam(teams.Last().Replace(" ", "")),
                        TotalElement = tableElement.FindElements(By.CssSelector(".coefficient .coefficient__cell"))[1].FindElements
                            (By.CssSelector(".coefficient__td"))[1]
                    });
                }
                catch (Exception ex)
                {
                    errBuilder.AppendLine(ex.Message);
                }
            });

            return results.ToArray();
        }

        public IWebElement GetHadicapRow(string[] teamNames)
        {
            ReadOnlyCollection<IWebElement> tableElements =
                WebDriver1.FindElements(By.CssSelector(".events .table .table__item"));

            foreach (var tableElement in tableElements)
            {
                var teams = tableElement.FindElement(By.ClassName("statistic__team"))
                    .Text//.Replace("\r\n", string.Empty)
                    .Split(new[] { " - ", " − ", " — ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (teamNames.Any(t => teams.Any(t1 => string.Equals(t, t1, StringComparison.CurrentCultureIgnoreCase))))
                {
                    return tableElement
                        .FindElements(By.CssSelector(".coefficient .coefficient__cell"))[1]
                        .FindElements(By.CssSelector(".coefficient__td"))[1];
                }
            }

            return null;
        }

        public WinlineRow[] GetHadicapRows(StringBuilder errBuilder)
        {
            SwitchBasketHcTab();

            var results = new List<WinlineRow>();
            ReadOnlyCollection<IWebElement> tableElements =
                WebDriver1.FindElements(By.CssSelector(".events .table .table__item"));

            Parallel.For(0, tableElements.Count, index =>
            {
                var tableElement = tableElements[index];
                try
                {
                    var teams = tableElement.FindElement(By.ClassName("statistic__team"))
                        .Text//.Replace("\r\n", string.Empty)
                        .Split(new[] { " - ", " − ", " — ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    results.Add(new WinlineRow
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams.First().Replace(" ", "")),
                        Team2 = TeamsHolder.Instance.GetTeam(teams.Last().Replace(" ", "")),
                        HandicapElement = tableElement
                            .FindElements(By.CssSelector(".coefficient .coefficient__cell"))[1]
                            .FindElements(By.CssSelector(".coefficient__td"))[1]
                    });
                }
                catch
                {
                    // ignored
                }
            });

            return results.ToArray();
        }

        public void Dispose()
        {
            WebDriver.Dispose();
            WebDriver1.Dispose();
        }

        #region private

        private void Auth(IWebDriver webDriver)
        {
            var login = ConfigurationManager.AppSettings["wlLogin"];
            var pwd = ConfigurationManager.AppSettings["wlPwd"];
            
            var loginElement = webDriver.FindElement(By.CssSelector(".login__form input[type='text']"));
            loginElement.SendKeys(login);

            var pwdElement = webDriver.FindElement(By.CssSelector(".login__form input[type='password']"));
            pwdElement.SendKeys(pwd);

            var submitBtn = webDriver.FindElement(By.CssSelector(".login__form .login__btn"));
            submitBtn.Click();
        }
        #endregion
    }
}

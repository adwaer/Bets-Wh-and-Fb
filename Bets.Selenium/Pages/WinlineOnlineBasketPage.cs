using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
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
        public readonly IJavaScriptExecutor Js;
        private bool _tabSwitched;

        public WinlineOnlineBasketPage()
        {
            WebDriver = GetNewDriver(ConfigurationManager.AppSettings["wlDriver"]);
            WebDriver1 = GetNewDriver(ConfigurationManager.AppSettings["wlDriver"]);
            Js = (IJavaScriptExecutor)WebDriver1;

            Setup(WebDriver, ConfigurationManager.AppSettings["wlUrl"]);
            Setup(WebDriver1, ConfigurationManager.AppSettings["wlUrl"]);
        }

        public override IRow[] GetRows(StringBuilder errBuilder)
        {
            var winlineRows = GetTotalRows(errBuilder);
            var hadicapRows = GetHadicapRows(errBuilder);

            foreach (var winlineRow in winlineRows)
            {
                var hcRow = hadicapRows.SingleOrDefault(r => r.Team1.Equals(winlineRow.Team1) || r.Team2.Equals(winlineRow.Team2));
                if (hcRow != null)
                {
                    winlineRow.HandicapElement = hcRow.HandicapElement;
                }
            }

            return winlineRows;
        }

        private void SwitchTab()
        {
            if (!_tabSwitched)
            {
                while (!_tabSwitched)
                {
                    try
                    {
                        Js.ExecuteScript(
                            "(function() { document.getElementsByClassName('event-tabs__link')[1].click(); })()");

                        _tabSwitched = true;
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
            var results = new List<WinlineRow>();
            ReadOnlyCollection<IWebElement> tableElements = WebDriver.FindElements(By.CssSelector(".events .table .table__item"));

            Parallel.For(0, tableElements.Count, index =>
            {
                var tableElement = tableElements[index];
                try
                {
                    var teams = tableElement.FindElement(By.ClassName("statistic__team"))
                        .Text
                        .Replace("\r\n", string.Empty)
                        .Split(new[] { '-', '−' }, StringSplitOptions.RemoveEmptyEntries);

                    results.Add(new WinlineRow
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams.First()),
                        Team2 = TeamsHolder.Instance.GetTeam(teams.Last()),
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

        public WinlineRow[] GetHadicapRows(StringBuilder errBuilder)
        {
            SwitchTab();

            var results = new List<WinlineRow>();
            ReadOnlyCollection<IWebElement> tableElements =
                WebDriver1.FindElements(By.CssSelector(".events .table .table__item"));

            Parallel.For(0, tableElements.Count, index =>
            {
                var tableElement = tableElements[index];
                try
                {
                    var teams = tableElement.FindElement(By.ClassName("statistic__team"))
                        .Text.Replace("\r\n", string.Empty)
                        .Split(new[] {" - ", " − ", " — " }, StringSplitOptions.RemoveEmptyEntries);

                    results.Add(new WinlineRow
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams.First()),
                        Team2 = TeamsHolder.Instance.GetTeam(teams.Last()),
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
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bets.Domain.PageElements;
using Bets.Services;
using OpenQA.Selenium;

namespace Bets.Selenium.Pages
{
    public class WinlineOnlineBasketPage
    {
        public readonly IWebDriver WebDriver;
        public readonly IJavaScriptExecutor Js;
        private bool _tabSwitched;

        public WinlineOnlineBasketPage(IWebDriver webDriver, string url)
        {
            WebDriver = webDriver;
            Js = (IJavaScriptExecutor)WebDriver;

            Setup(url);
        }

        private void Setup(string url)
        {
            Thread.Sleep(10000);
            WebDriver
                .Navigate()
                .GoToUrl(url);
            Thread.Sleep(10000);
        }

        public IEnumerable<WinlineRow> GetTotalRows(StringBuilder errBuilder)
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

            return results;
        }

        public IEnumerable<WinlineRow> GetHadicapRows(StringBuilder errBuilder)
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


            var results = new List<WinlineRow>();
            ReadOnlyCollection<IWebElement> tableElements =
                WebDriver.FindElements(By.CssSelector(".events .table .table__item"));

            Parallel.For(0, tableElements.Count, index =>
            {
                var tableElement = tableElements[index];
                try
                {
                    var teams = tableElement.FindElement(By.ClassName("statistic__team"))
                        .Text.Replace("\r\n", string.Empty)
                        .Split(new[] {'-', '−'}, StringSplitOptions.RemoveEmptyEntries);

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

            return results;
        }
        
    }
}

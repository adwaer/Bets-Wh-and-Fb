using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bets.Domain.PageElements;
using Bets.Services;
using OpenQA.Selenium;

namespace Bets.Selenium.Pages
{
    public sealed class FonbetOnlineBasketPage : BasketPage, IDisposable
    {
        public readonly IWebDriver WebDriver;
        public readonly IJavaScriptExecutor Js;

        public FonbetOnlineBasketPage()
        {
            WebDriver = GetNewDriver(ConfigurationManager.AppSettings["fonbetDriver"]);
            Setup(WebDriver, ConfigurationManager.AppSettings["fonbetUrl"]);
            Js = (IJavaScriptExecutor)WebDriver;
        }

        public override IRow[] GetRows(StringBuilder errBuilder)
        {
            var webElements = WebDriver.FindElement(By.Id("lineContainer"))
                .FindElements(By.ClassName("trEvent"))
                .Where(webElement => webElement.GetAttribute("style").Contains("display: table-row;"));

            var fonbetRows = new List<FonbetRow>();
            Parallel.ForEach(webElements, webElement =>
            {
                try
                {
                    string[] teams;
                    try
                    {
                        teams = webElement.FindElement(By.ClassName("event"))
                            .Text.Split(new[] { " - ", " − ", " — " }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    catch (NoSuchElementException)
                    {
                        return;
                    }

                    var readOnlyCollection = webElement.FindElements(By.ClassName("eventCellParam"));
                    fonbetRows.Add(new FonbetRow
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams[0].Replace(" ", "")),
                        Team2 = TeamsHolder.Instance.GetTeam(teams[1].Replace(" ", "")),
                        TotalElement = readOnlyCollection[2],
                        HandicapElement = readOnlyCollection[1]
                    });
                }
                catch
                {
                    // ignored
                }
            });

            return fonbetRows.ToArray();
        }

        public void Dispose()
        {
            WebDriver.Dispose();
        }
    }
}

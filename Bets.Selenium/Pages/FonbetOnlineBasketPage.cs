using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bets.Domain.PageElements;
using Bets.Services;
using OpenQA.Selenium;

namespace Bets.Selenium.Pages
{
    public class FonbetOnlineBasketPage
    {
        public readonly IWebDriver WebDriver;
        public readonly IJavaScriptExecutor Js;

        public FonbetOnlineBasketPage(IWebDriver webDriver, string url)
        {
            WebDriver = webDriver;
            Js = (IJavaScriptExecutor)WebDriver;
            Setup(url);
        }

        private void Setup(string url)
        {
            WebDriver
                .Navigate()
                .GoToUrl(url);
        }

        public IEnumerable<FonbetRow> GetFonbetRows()
        {
            var webElements = WebDriver.FindElement(By.Id("lineContainer"))
                .FindElements(By.ClassName("trEvent"));

            var fonbetRows = new List<FonbetRow>();
            Parallel.ForEach(webElements, webElement =>
            {
                try
                {
                    if (webElement.GetAttribute("style") == "display: none")
                    {
                        return;
                    }

                    string[] teams;
                    try
                    {
                        teams = webElement.FindElement(By.ClassName("event"))
                            .Text.Split(new[] { '—' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    catch (NoSuchElementException)
                    {
                        return;
                    }

                    fonbetRows.Add(new FonbetRow(webElement.FindElements(By.ClassName("eventCellParam")))
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams[0]),
                        Team2 = TeamsHolder.Instance.GetTeam(teams[1])
                    });
                }
                catch
                {
                    // ignored
                }
            });

            return fonbetRows;
        }
        
    }
}

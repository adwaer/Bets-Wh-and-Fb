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
        public FonbetOnlineBasketPage()
        {
            WebDriver = GetNewDriver(ConfigurationManager.AppSettings["fonbetDriver"]);
            Setup(WebDriver, ConfigurationManager.AppSettings["fonbetUrl"]);
            Auth(WebDriver);
        }

        public override IRow[] GetRows(StringBuilder errBuilder)
        {
            IEnumerable<IWebElement> webElements = null;
            while (webElements == null)
            {
                try
                {
                    webElements = WebDriver.FindElement(By.Id("lineContainer"))
                        .FindElements(By.ClassName("trEvent"))
                        .Where(webElement => webElement.GetAttribute("style").Contains("display: table-row;"));
                }
                catch
                {
                    // ignored
                }
            }
            
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

        #region private

        private void Auth(IWebDriver webDriver)
        {
            var login = ConfigurationManager.AppSettings["fbLogin"];
            var pwd = ConfigurationManager.AppSettings["fbPwd"];
            
            var loginElement = webDriver.FindElement(By.Id("editLogin"));
            loginElement.SendKeys(login);

            var pwdElement = webDriver.FindElement(By.Id("editPassword"));
            pwdElement.SendKeys(pwd);

            var submitBtn = webDriver.FindElement(By.Id("loginButtonLogin"));
            submitBtn.Click();
        }
        #endregion
    }
}

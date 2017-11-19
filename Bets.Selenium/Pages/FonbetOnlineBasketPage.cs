using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bets.Domain;
using Bets.Domain.PageElements;
using OpenQA.Selenium;

namespace Bets.Selenium.Pages
{
    public sealed class FonbetOnlineBasketPage : BasketPage, IDisposable
    {
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
                        .FindElements(By.ClassName("trEvent"));
                    webElements = webElements.Where(webElement => webElement.GetAttribute("style").Contains("display: table-row;"));
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
                    var columns = webElement.FindElements(By.TagName("td"));
                    string[] teams;
                    try
                    {
                        teams = columns[2].Text.Split(new[] { " - ", " − ", " — " }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    catch (NoSuchElementException)
                    {
                        return;
                    }
                    
                    fonbetRows.Add(new FonbetRow
                    {
                        Team1 = TeamsHolder.Instance.GetTeam(teams[0].Replace(" ", "")),
                        Team2 = TeamsHolder.Instance.GetTeam(teams[1].Replace(" ", "")),
                        TotalElement = columns[13],
                        TotalLessElement = columns[15],
                        TotalMoreElement = columns[14],
                        HandicapElement = columns[11],
                        HandicapLessElement = columns[10],
                        HandicapMoreElement = columns[12]
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

        protected override void ClearBets()
        {
            var webElements = WebDriver.FindElements(By.ClassName("buttonDelete"));
            foreach (var webElement in webElements)
            {
                webElement.Click();
            }
        }

        #region private

        private void Auth(IWebDriver webDriver)
        {
            var login = ConfigurationManager.AppSettings["fbLogin"];
            var pwd = ConfigurationManager.AppSettings["fbPwd"];
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pwd))
            {
                return;
            }

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

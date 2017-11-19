using OpenQA.Selenium;

namespace Bets.Domain.PageElements
{
    public class FonbetRow : IRow
    {
        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }

        public IWebElement TotalElement { get; set; }
        public IWebElement TotalLessElement { get; set; }
        public IWebElement TotalMoreElement { get; set; }

        public IWebElement HandicapElement { get; set; }
        public IWebElement HandicapLessElement { get; set; }
        public IWebElement HandicapMoreElement { get; set; }
    }
}

using OpenQA.Selenium;

namespace Bets.Domain.PageElements
{
    public interface IRow
    {
        TeamViewModel Team1 { get; set; }
        TeamViewModel Team2 { get; set; }
        IWebElement TotalElement { get; set; }
        IWebElement TotalLessElement { get; set; }
        IWebElement TotalMoreElement { get; set; }
        IWebElement HandicapElement { get; set; }
        IWebElement HandicapLessElement { get; set; }
        IWebElement HandicapMoreElement { get; set; }
    }
}

using Bets.Domain.PageElements;
using OpenQA.Selenium;

namespace Bets.Domain
{
    public class StatViewModel
    {
        public StatViewModel(IRow game)
        {
            TotalWebElement = game.TotalElement;
            try
            {
                Total = game.TotalElement.Text;
            }
            catch
            {
                // ignored
            }
            HandicapWebElement = game.HandicapElement;
            try
            {
                Handicap = game.HandicapElement.Text;
            }
            catch
            {
                // ignored
            }
        }

        public string Total { get; set; }
        public IWebElement TotalWebElement { get; set; }

        public string Handicap { get; set; }
        public IWebElement HandicapWebElement { get; set; }
    }
}

using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Bets.Domain.PageElements
{
    public class FonbetRow : IRow
    {
        private readonly ReadOnlyCollection<IWebElement> _webElements;

        public FonbetRow(ReadOnlyCollection<IWebElement> webElements)
        {
            _webElements = webElements;
        }

        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }

        public IWebElement TotalElement => _webElements[2];
        public IWebElement HandicapElement => _webElements[1];
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Bets.Domain.Annotations;
using Bets.Domain.PageElements;
using OpenQA.Selenium;

namespace Bets.Domain
{
    public class StatViewModel
    {
        public IWebElement TotalWebElement { get; set; }
        public IWebElement HandicapWebElement { get; set; }

        public ObservableObject<string> Total { get; set; }
        public ObservableObject<string> Handicap { get; set; }

        public StatViewModel(IRow game)
        {
            TotalWebElement = game.TotalElement;
            HandicapWebElement = game.HandicapElement;

            Total = new ObservableObject<string>();
            Handicap = new ObservableObject<string>();

            Update();
        }

        public void Update()
        {
            string currentTotal, currentHc;

            try
            {
                if (string.IsNullOrEmpty(TotalWebElement.Text))
                {
                    currentTotal = @"-\-";
                }
                else
                {
                    currentTotal = TotalWebElement.Text;
                }
            }
            catch
            {
                currentTotal = "-X-";
            }
            if (!string.Equals(currentTotal, Total.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                Total.Value = currentTotal;
            }
            //else if (!TotalColor.Equals(Brushes.Transparent))
            //{
            //    TotalColor = Brushes.Transparent;
            //}

            try
            {
                if (string.IsNullOrEmpty(HandicapWebElement.Text))
                {
                    currentHc = @"-\-";
                }
                else
                {
                    currentHc = HandicapWebElement
                        .Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
                }
            }
            catch
            {
                currentHc = "-X-";
            }

            if (!string.Equals(currentHc, Handicap.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                Handicap.Value = currentHc;
            }
            //else if (!HandicapColor.Equals(Brushes.Transparent))
            //{
            //    HandicapColor = Brushes.Transparent;
            //}

        }
        
    }
}

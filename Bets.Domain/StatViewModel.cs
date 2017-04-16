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

        public TotalObject Total { get; set; }
        public ObservableObject<string> Handicap { get; set; }

        public StatViewModel(IRow game)
        {
            TotalWebElement = game.TotalElement;
            HandicapWebElement = game.HandicapElement;

            Total = new TotalObject();
            Handicap = new ObservableObject<string>();

            Update();
        }

        public void Update()
        {
            try
            {
                if (string.IsNullOrEmpty(TotalWebElement.Text))
                {
                    Total.Value = @"-\-";
                }
                else
                {
                    Total.Value = TotalWebElement.Text;
                }
            }
            catch
            {
                Total.Value = "-X-";
            }

            try
            {
                if (string.IsNullOrEmpty(HandicapWebElement.Text))
                {
                    Handicap.Value = @"-\-";
                }
                else
                {
                    Handicap.Value = HandicapWebElement
                        .Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
                }
            }
            catch
            {
                Handicap.Value = "-X-";
            }
            

        }
        
    }
}

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
        public IRow Game { get; }

        public TotalObject Total { get; set; }
        public ObservableObject<string> Handicap { get; set; }

        public StatViewModel(IRow game)
        {
            Game = game;
            Total = new TotalObject();
            Handicap = new ObservableObject<string>();

            Update();
        }

        public void Update()
        {
            try
            {
                if (string.IsNullOrEmpty(Game.TotalElement.Text))
                {
                    Total.Value = @"-\-";
                }
                else
                {
                    Total.Value = Game.TotalElement.Text;
                }
            }
            catch
            {
                Total.Value = "-X-";
            }

            try
            {
                if (string.IsNullOrEmpty(Game.HandicapElement.Text))
                {
                    Handicap.Value = @"-\-";
                }
                else
                {
                    Handicap.Value = Game.HandicapElement
                        .Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0]
                        .Replace("−", "")
                        .Replace("-", "")
                        .Replace("+", "");
                }
            }
            catch
            {
                Handicap.Value = "-X-";
            }
            

        }
        
    }
}

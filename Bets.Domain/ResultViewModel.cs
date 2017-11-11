using System;
using System.Configuration;
using System.Globalization;

namespace Bets.Domain
{
    public class ResultViewModel
    {
        public bool AutoBettingTotal { get; set; } = false;
        public decimal AmountTotal { get; set; } = decimal.Parse(ConfigurationManager.AppSettings["defaultAmountTotal"]);
        public decimal CefTotal { get; set; } = decimal.Parse(ConfigurationManager.AppSettings["defaultCefTotal"]);
        public bool AutoBettingHandicap { get; set; } = false;
        public decimal AmountHandicap { get; set; } = decimal.Parse(ConfigurationManager.AppSettings["defaultAmountHandicap"]);
        public decimal CefHandicap { get; set; } = decimal.Parse(ConfigurationManager.AppSettings["defaultCefHandicap"]);

        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }

        public StatViewModel Winline { get; set; }
        public StatViewModel WinlinePrev { get; set; }
        public StatViewModel Fonbet { get; set; }
        public StatViewModel FonbetPrev { get; set; }

        public void Update()
        {
            Winline.Update();
            Fonbet.Update();

            IsGoodTotal.Value = CalcIsGoodTotal();
            IsGoodHc.Value = CalcIsGoodHc();

            WinlinePrev = Winline;
            FonbetPrev = Fonbet;
        }

        public bool IsTotalNotPrev()
        {
            return WinlinePrev == null
                || FonbetPrev == null
                || WinlinePrev.Total.Value != Winline.Total.Value
                || FonbetPrev.Total.Value != Fonbet.Total.Value;
        }
        public bool IsHcNotPrev()
        {
            return WinlinePrev == null
                   || FonbetPrev == null
                   || WinlinePrev.Handicap.Value != Winline.Handicap.Value
                   || FonbetPrev.Handicap.Value != Fonbet.Handicap.Value;
        }

        public ObservableObject<int> IsGoodTotal { get; } = new ObservableObject<int>();
        public ObservableObject<int> IsGoodHc { get; } = new ObservableObject<int>();
        private int CalcIsGoodTotal()
        {
            if (!AutoBettingTotal
                || AmountTotal == 0
                || CefTotal == 0)
            {
                return 0;
            }
            if (!decimal.TryParse(Winline.Total.Value, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture.NumberFormat, out decimal totalWl))
            {
                return 0;
            }
            totalWl = Math.Abs(totalWl);
            if (!decimal.TryParse(Fonbet.Total.Value, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture.NumberFormat, out decimal totalFb))
            {
                return 0;
            }
            totalFb = Math.Abs(totalFb);

            var diff = Math.Abs(totalWl - totalFb);
            var cef = Math.Abs(CefTotal);

            if (diff >= cef)
            {
                return totalWl > totalFb ? -1 : 1;
            }
            return 0;
        }

        private int CalcIsGoodHc()
        {
            if (!AutoBettingHandicap
                || AmountHandicap == 0
                || CefHandicap == 0)
            {
                return 0;
            }
            if (!decimal.TryParse(Winline.Handicap.Value, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture.NumberFormat, out decimal hcWl))
            {
                return 0;
            }
            hcWl = Math.Abs(hcWl);
            if (!decimal.TryParse(Fonbet.Handicap.Value, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture.NumberFormat, out decimal hcFb))
            {
                return 0;
            }
            hcFb = Math.Abs(hcFb);

            var diff = Math.Abs(hcWl - hcFb);
            var cef = Math.Abs(CefHandicap);

            if (diff >= cef)
            {
                return hcWl > hcFb ? -1 : 1;
            }
            return 0;
        }
    }
}
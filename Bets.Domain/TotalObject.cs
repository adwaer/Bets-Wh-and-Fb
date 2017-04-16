using System;
using System.Globalization;

namespace Bets.Domain
{
    public class TotalObject : ObservableObject<string>
    {
        public TotalObject CompareObject { get; set; }



        private bool _goodTotal;
        public bool GoodTotal
        {
            get
            {
                return _goodTotal;
            }
            set
            {
                _goodTotal = value;
                OnPropertyChanged();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Value))
            {
                decimal total1, total2;
                if (decimal.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out total1) &&
                    decimal.TryParse(CompareObject.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out total2))
                {
                    if (Math.Abs(total1 - total2) >= 2)
                    {
                        GoodTotal = true;
                    }
                    //else if (_goodTotal)
                    //{
                    //    GoodTotal = false;
                    //}
                }
            }
        }
    }
}
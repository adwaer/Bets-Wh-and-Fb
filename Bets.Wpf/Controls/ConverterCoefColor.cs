using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace Bets.Wpf.Controls
{
    public class ConverterCoefColor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var s1 = (string) values[0];
            s1 = s1.Replace("-", string.Empty).Replace("−", string.Empty).Replace("—", string.Empty);
            var s2 = (string)values[1];
            s2 = s2.Replace("-", string.Empty).Replace("−", string.Empty).Replace("—", string.Empty);

            decimal coef, val1, val2;
            if(!decimal.TryParse((string)parameter, NumberStyles.Any, CultureInfo.InvariantCulture, out coef) ||
                !decimal.TryParse(s1, NumberStyles.Any, CultureInfo.InvariantCulture, out val1) ||
                !decimal.TryParse(s2, NumberStyles.Any, CultureInfo.InvariantCulture, out val2))
            {
                return Brushes.MistyRose;
            }
            
            return Math.Abs(val1 - val2) >= coef ? Brushes.Aqua : Brushes.Transparent;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
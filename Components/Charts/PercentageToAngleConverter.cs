using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace LogAnalysisTool.Components.Charts
{
    public class PercentageToAngleConverter : IValueConverter
    {
        #region IValueConverter Implementation
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 360 * (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 360;
        }
        #endregion
    }
}

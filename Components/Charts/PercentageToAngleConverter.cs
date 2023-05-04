using System;
using System.Globalization;
using System.Windows.Data;

namespace LogAnalysisTool.Components.Charts
{
    /// <summary>
    /// PercentageToAngleConverter.cs by Charles Petzold, June 2009 https://learn.microsoft.com/en-us/archive/msdn-
    /// magazine/2009/september/charting-with-datatemplates
    /// </summary>
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

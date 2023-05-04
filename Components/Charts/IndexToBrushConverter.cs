using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LogAnalysisTool.Components.Charts
{
    /// <summary>
    /// IndexToBrushConverter.cs by Charles Petzold, June 2009 https://learn.microsoft.com/en-us/archive/msdn-
    /// magazine/2009/september/charting-with-datatemplates
    /// </summary>
    public class IndexToBrushConverter : IValueConverter
    {
        public Brush[] Brushes { get; set; }

        #region IValueConverter Implementation
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Brushes.Length == 0)
            {
                return null;
            }

            return Brushes[(int)value % Brushes.Length];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace LogAnalysisTool.Components.Charts
{
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

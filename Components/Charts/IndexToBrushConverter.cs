using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LogAnalysisTool.Components.Charts
{
    public class IndexToBrushConverter : IValueConverter
    {
        public Brush[] Brushes { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Brushes.Length == 0)
                return null;

            return Brushes[(int)value % Brushes.Length];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

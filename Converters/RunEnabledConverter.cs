using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LogAnalysisTool.Converters
{
    /// <summary>
    /// RunEnabledConverter - contains logic to determine whether the run button on the analysis view model should be
    /// enabled depending on certain criteria being met
    /// </summary>
    public class RunEnabledConverter : IMultiValueConverter
    {
        /// <summary>
        /// Convert() - method to determine if Run button should be enabled based on certain criteria being met
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length != 3)
                return false;
            if (values[0] is not string)
                return false;
            string fileName = (string)values[0];
            var exists = System.IO.File.Exists(fileName);
            if (exists == false)
                return false;

            if (values[1] is null)
                return false;

            if (values[2] is not bool)
                return false;
            bool runningAnalysis = (bool)values[2];

            return !runningAnalysis;
        }

        /// <summary>
        /// ConvertBack() - method to determine if the Run button should be converted back to a disabled state
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

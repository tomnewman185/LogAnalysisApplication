using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests.MachineLearning.Training
{
    internal class TrainingDataLogReader
    {
        // Method to fetch data from the training data file
        public static IEnumerable<ApacheLogFileLabels> GetData(string dataFileName, string labelFileName, Regex regexForLogFile)
        {
            // Creates a new StreamReader used to read in the log data from the training data file
            using (var dataReader = new StreamReader(dataFileName))
            {
                // Creates a new StreamReader used to read in the label data from the corresponding training data label file
                using (var labelReader = new StreamReader(labelFileName))
                {
                    // While loop keeping the loop open until the end of the file is reached
                    while (!dataReader.EndOfStream)
                    {
                        // Read in log data from the training data file
                        var dataLine = dataReader.ReadLine();
                        Match m = regexForLogFile.Match(dataLine);

                        // Read in label data from the corresponding training data label file
                        var labelLine = labelReader.ReadLine();

                        // As there are two labels for each log entry in the training dataset and they are seperated by a comma, each character is split apart using the comma as the seperating character
                        var labelParts = labelLine.Split(new char[] { ',' });

                        // In the lables file, the labels are 0 if they represent normal entries, or are named after the attack they represent. Due to the machine learning algorithm selected needing a boolean input, if the label is not of value 0, we instead represent it as a 1
                        if (labelParts[0] != "0")
                        {
                            labelParts[0] = "1";
                        }

                        // Convert the label to a boolean value
                        bool bVal = Convert.ToBoolean(Convert.ToInt16(labelParts[0]));

                        // Create a new data item of type ApacheLogFileLables using the data gathered from the readers
                        var data = new ApacheLogFileLabels()
                                   {
                                       RemoteHost = m.Groups[ApacheLogComponents.RegexComponentGroups.RemoteHost].Value,
                                       RFC931 = m.Groups[ApacheLogComponents.RegexComponentGroups.RFC931].Value,
                                       AuthUser = m.Groups[ApacheLogComponents.RegexComponentGroups.AuthUser].Value,
                                       Date = m.Groups[ApacheLogComponents.RegexComponentGroups.Date].Value,
                                       Request = m.Groups[ApacheLogComponents.RegexComponentGroups.Request].Value,
                                       Status = m.Groups[ApacheLogComponents.RegexComponentGroups.Status].Value,
                                       Bytes = m.Groups[ApacheLogComponents.RegexComponentGroups.Bytes].Value,
                                       Referer = m.Groups[ApacheLogComponents.RegexComponentGroups.Referer].Value,
                                       UserAgent = m.Groups[ApacheLogComponents.RegexComponentGroups.UserAgent].Value,
                                       Label = bVal,
                                       Label2 = labelParts[1]
                                   };

                        yield return data;
                    }
                }
            }
        }
    }
}

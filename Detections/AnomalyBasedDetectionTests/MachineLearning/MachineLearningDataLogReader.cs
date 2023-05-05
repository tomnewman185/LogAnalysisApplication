using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogAnalysisTool.Detections.AnomalyBasedDetectionTests.MachineLearning
{
    public class MachineLearningDataLogReader<T>
    {
        // Method to fetch data from the log file
        public static IEnumerable<T> GetData(string dataFileName, Regex regexForLogFile, Func<Match, int, T> CreateDataItem)
        {
            int lineCounter = 0;

            // Creates a new StreamReader used to read in the data from the file
            using (var dataReader = new StreamReader(dataFileName))
            {
                // While loop keeping the loop open until the end of the file is reached
                while (!dataReader.EndOfStream)
                {
                    // Increment line counter for each line
                    lineCounter++;

                    // Read line
                    var dataLine = dataReader.ReadLine();
                    Match m = regexForLogFile.Match(dataLine);

                    if (!m.Success)
                    {
                        throw new Exception($"Log File Regex is not a match for a the log line {dataLine}");
                    }

                    // Create new object and populate with values from read in files, storing it in an enumerable
                    var data = CreateDataItem(m, lineCounter);
                    yield return data;
                }
            }
        }
    }
}

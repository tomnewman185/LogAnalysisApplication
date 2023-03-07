using LogAnalysisTool.Apache_Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogAnalysisTool
{
    internal class LogReader
    {
        public static IEnumerable<MaliciousLogEntryInfo> Read(string fileName, LogType logType)
        {
            //Creation of new StreamReader to read from the inputted file
            using (var file = new StreamReader(fileName))
            {
                //Retrieves the type of log that is being analysed
                ILogTypeInfo logTypeInfo = LogTypeFactory.GetLogType(logType);

                //Retrieves the regular expression for the type of log being analysed, which breaks the log down into its components
                Regex regex = new Regex(logTypeInfo.RegularExpression);

                int lineCounter = 0;

                //While loop to read in the lines of the file whilst the end of the file has not been reached
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();

                    lineCounter++;

                    //For each match of a behavioural detection test, we create a new detection if malicious log has been detected
                    foreach(Match match in regex.Matches(line))
                    {
                        foreach(IBehaviouralDetectionTest behaviouralDetectionTest in logTypeInfo.GetBehaviouralDetectionTests())
                        {
                            var detections = behaviouralDetectionTest.ConductTest(match, line, lineCounter);

                            foreach (var detection in detections)
                            {
                                yield return detection;
                            }
                        }
                    }
                }
            }
        }
    }
}

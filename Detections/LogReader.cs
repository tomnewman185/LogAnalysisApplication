using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogAnalysisTool.Detections.BehaviourBasedDetectionTests;

namespace LogAnalysisTool.Detections
{
    /// <summary>
    /// LogReader - Logic for all tasks related to reading in data from a log file
    /// </summary>
    internal class LogReader
    {
        /// <summary>
        /// CountLinesAsync() - method to count the number of lines within a log file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<int> CountLinesAsync(string fileName)
        {
            // Creates a new StreamReader
            using (var rdr = new StreamReader(fileName))
            {
                int lineCounter = 0;

                while (!rdr.EndOfStream)
                {
                    await rdr.ReadLineAsync();
                    lineCounter++;
                }

                return lineCounter;
            }
        }

        /// <summary>
        /// GetDetections() - Logic behind conducting the behavioural-based detection tests
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineCount"></param>
        /// <param name="regex"></param>
        /// <param name="behaviouralTests"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<MaliciousLogEntryInfo> GetDetections(string line,
                                                                       int lineCount,
                                                                       Regex regex,
                                                                       IEnumerable<IBehaviouralDetectionTest> behaviouralTests)
        {
            MatchCollection matches = regex.Matches(line);

            if (matches.Count == 0)
            {
                throw new Exception($"No matches found for a the log line {line}");
            }

            foreach (Match match in matches)
            {
                // Defensively check for a successful match
                if (!match.Success)
                {
                    throw new Exception($"Regex Match is not a successful match for a the log line {line}");
                }

                // For each behavioural-based detection test, if there is a detection, return a new detection
                foreach (IBehaviouralDetectionTest behaviouralTest in behaviouralTests)
                {
                    var detections = behaviouralTest.ConductTest(match, line, lineCount);

                    foreach (var detection in detections)
                    {
                        yield return detection;
                    }
                }
            }
        }

        /// <summary>
        /// ReadLinesAsync() - method to read in lines from a log file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<string> ReadLinesAsync(string fileName)
        {
            using (var file = new StreamReader(fileName))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLineAsync();

                    yield return await line;
                }
            }
        }
    }
}

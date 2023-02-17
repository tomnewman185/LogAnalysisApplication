using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LogAnalysisTool
{
    //Class to define information that we want to collect regarding a malicious log entry
    internal class MaliciousLogEntryInfo
    {
        public string LogEntry { get; }
        public Match Match { get; }
        public Group Group { get; }
        public int IndexWithinGroup { get; }
        public int LineNumber { get; }
        public int LengthOfDetection { get; }
        public IDetectionTest TestDetected { get; }

        public MaliciousLogEntryInfo(string logEntry, Match match, Group group, int indexWithinGroup, int lineNumber, int lengthOfDetection, IDetectionTest testDetected) 
        {
            LogEntry = logEntry;
            Match = match;
            Group = group;
            IndexWithinGroup = indexWithinGroup;
            LineNumber = lineNumber;
            LengthOfDetection = lengthOfDetection;
            TestDetected = testDetected;
        }
    }
}

using System;
using System.Linq;
using System.Text.RegularExpressions;
using LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests.Apache;

namespace LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests
{
    /// <summary>
    /// MaliciousLogEntryInfo - Class to define information that we want to collect regarding a malicious log entry
    /// </summary>
    internal class MaliciousLogEntryInfo : IBehaviouralDetectionItem
    {
        public MaliciousLogEntryInfo(string logEntry,
                                     Match match,
                                     Group group,
                                     int indexWithinGroup,
                                     int lineNumber,
                                     int lengthOfDetection,
                                     ITest testDetected,
                                     bool fromTorExitNode)
        {
            LogEntry = logEntry;
            Match = match;
            Group = group;
            IndexWithinGroup = indexWithinGroup;
            LineNumber = lineNumber;
            LengthOfDetection = lengthOfDetection;
            TestDetected = testDetected;
            FromTorExitNode = fromTorExitNode;
        }

        public bool FromTorExitNode { get; }

        public Group Group { get; }

        public int IndexWithinGroup { get; }

        public int LengthOfDetection { get; }

        public int LineNumber { get; }

        public string LogEntry { get; }

        public Match Match { get; }

        public ITest TestDetected { get; }

        #region IDetectionItem Implementation
        int IDetectionItem.LineNumber => LineNumber;

        string IDetectionItem.Request => Match.Value;
        #endregion
    }
}

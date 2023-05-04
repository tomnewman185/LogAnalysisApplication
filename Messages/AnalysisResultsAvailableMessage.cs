using System;
using System.Collections.Generic;
using System.Linq;
using LogAnalysisTool.LogsFiles.AnomalyBasedDetectionTests;
using LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests;

namespace LogAnalysisTool.Messages
{
    /// <summary>
    /// AnalysisResultsAvailableMessage - sends a message when the analysis results are available
    /// </summary>
    internal class AnalysisResultsAvailableMessage
    {
        internal AnalysisResultsAvailableMessage(string fileName,
                                                 IEnumerable<MaliciousLogEntryInfo> vulnerabilities,
                                                 IEnumerable<IAnomalyDetectionItem> mlVulnerabilities,
                                                 int totalLogLineCount)
        {
            TotalLogLineCount = totalLogLineCount;
            FileName = fileName;
            Vulnerabilities = new List<MaliciousLogEntryInfo>(vulnerabilities);
            MLVulnerabilities = new List<IAnomalyDetectionItem>(mlVulnerabilities);
        }

        public string FileName { get; private set; }

        public List<IAnomalyDetectionItem> MLVulnerabilities { get; private set; }

        public int TotalLogLineCount { get; private set; }

        public List<MaliciousLogEntryInfo> Vulnerabilities { get; private set; }
    }
}

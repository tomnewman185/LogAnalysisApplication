using System;
using System.Collections.Generic;
using System.Linq;
using LogAnalysisTool.Detections.BehaviourBasedDetectionTests;

namespace LogAnalysisTool.StatisticReporting
{
    internal static class StatisticsHelper

    {
        // Extension method to get information about the most frequently occuring behaviorual detection
        public static (string testName, int maxCount) GetMaxLogIssueInfo(this IEnumerable<MaliciousLogEntryInfo> logIssues)
        {
            var q = logIssues.GroupBy(i => i.TestDetected.Name).MaxBy(g => g.Count());
            return (q.Key, q.Count());
        }

        // Extenstion method to get information about the least frequently occuring behaviorual detection
        public static (string testName, int minCount) GetMinLogIssueInfo(this IEnumerable<MaliciousLogEntryInfo> logIssues)
        {
            var q = logIssues.GroupBy(i => i.TestDetected.Name).MinBy(g => g.Count());
            return (q.Key, q.Count());
        }

        // Extension method to get the statistics for the pie chart shown in the StatisticsView
        public static IEnumerable<(string Label, double Percentage, int Count)> GetPieChartStats(this IEnumerable<MaliciousLogEntryInfo> logIssues)
        {
            var data = logIssues.GroupBy(i => i.TestDetected.Name).Select(g => (g.Key, g.Count() / (double)logIssues.Count(), g.Count()));

            return data;
        }
    }
}

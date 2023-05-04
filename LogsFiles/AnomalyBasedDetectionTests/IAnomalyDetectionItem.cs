using System;
using System.Linq;
using LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests;

namespace LogAnalysisTool.LogsFiles.AnomalyBasedDetectionTests
{
    /// <summary>
    /// IAnomalyDetectionItem - Implemented on types that represent a detected threat that is anomaly based.
    /// </summary>
    public interface IAnomalyDetectionItem : IDetectionItem
    {
    }
}

using System;
using System.Linq;

namespace LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests
{
    /// <summary>
    /// IAnomalyDetectionItem - Implemented on types that represent a detected threat that is anomaly based.
    /// </summary>
    public interface IAnomalyDetectionItem : IDetectionItem
    {
    }
}

using System;
using System.Linq;

namespace LogAnalysisTool.LogsFiles.BehaviourBasedDetectionTests
{
    /// <summary>
    /// IDetectionItem - Implemented on types that represent a detected threat, whether that is bahavioural or anomaly
    /// based.
    /// </summary>
    public interface IDetectionItem
    {
        public int LineNumber { get; }

        public string Request { get; }
    }
}

using System;
using System.Linq;

namespace LogAnalysisTool.LogsFiles.AnomalyBasedDetectionTests.MachineLearning.Apache.Training
{
    /// <summary>
    /// Result - Class for handling machine learning prediction results
    /// </summary>
    public class Result : ApacheLogFile
    {
        public bool PredictedLabel { get; set; }

        public float Score { get; set; }
    }
}

using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests.MachineLearning.Training
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

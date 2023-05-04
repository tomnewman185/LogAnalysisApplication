using System;
using System.Linq;

namespace LogAnalysisTool.LogsFiles.AnomalyBasedDetectionTests.MachineLearning.Apache.Training
{
    // Class that inherits properties from ApacheLogFile class but adds the label components for training the machine learning algorithm. 
    internal class ApacheLogFileLabels : ApacheLogFile
    {
        public bool Label { get; set; }

        public string Label2 { get; set; }
    }
}

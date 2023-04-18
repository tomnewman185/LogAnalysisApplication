using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests.MachineLearning.Training
{
    //Class that inherits properties from ApacheLogFile class but adds the label components for training the machine learning algorithm. 
    internal class ApacheLogFileLabels : ApacheLogFile
    {
        public bool Label { get; set; }
        public string Label2 { get; set; }

    }
}

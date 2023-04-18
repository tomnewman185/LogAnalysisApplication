using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests.MachineLearning.Training
{
    //Class defining the components of an Apache log file needed for the machine learning portion of the system
    public class ApacheLogFile : IAnomalyDetectionItem
    {
        public string RemoteHost { get; set; }
        public string RFC931 { get; set; }
        public string AuthUser { get; set; }
        public string Date { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }
        public string Bytes { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public int LineNumber { get; set; }

    }
}

using System;
using System.Linq;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public class ProgressReportInfo
    {
        public int PercentageDone { get; set; }

        public bool ShowAnomalyProgressBar { get; set; }
    }
}

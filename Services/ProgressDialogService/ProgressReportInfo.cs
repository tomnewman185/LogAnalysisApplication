using DevExpress.Mvvm.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public class ProgressReportInfo
    {
        public int PercentageDone { get; set; }

        public bool ShowAnomalyProgressBar { get; set; }
    }
}

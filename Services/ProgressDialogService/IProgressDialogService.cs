using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public interface IProgressDialogService
    {
        Task ShowDialog<T>(Func<CancellationToken, IProgress<ProgressReportInfo>, int, Task> action, int totalRowCount, bool isCancellable = true);
    }
}

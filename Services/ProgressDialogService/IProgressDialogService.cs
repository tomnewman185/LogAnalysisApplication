using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public interface IProgressDialogService
    {
        Task ShowDialog<T>(Func<CancellationToken, IProgress<ProgressReportInfo>, int, Task> action, int totalRowCount, bool isCancellable = true);
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Mvvm.UI;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public class ProgressDialogService : ServiceBase, IProgressDialogService
    {
        #region IProgressDialogService Implementation
        public async Task ShowDialog<T>(Func<CancellationToken, IProgress<ProgressReportInfo>, int, Task> action,
                                        int totalLogLineCount,
                                        bool isCancellable = true)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                var cancelCommand = isCancellable ? new CancelCommand(cancellationTokenSource) : null;

                var progrDialogViewModel = new ProgressDialogWindowViewModel("Progress", cancellationToken, cancelCommand);

                var window = new ProgressDialogWindow
                {
                    DataContext = progrDialogViewModel
                };

                Task task = action(cancellationToken, progrDialogViewModel.Progress, totalLogLineCount)
                            .ContinueWith(task =>
                                          {
                                              window.Dispatcher
                                                    .BeginInvoke(() =>
                                                                 {
                                                                     progrDialogViewModel.Close();
                                                                 });
                                              task.GetAwaiter().GetResult();
                                          });

                window.ShowDialog();

                await task;
            }
        }
        #endregion
    }
}

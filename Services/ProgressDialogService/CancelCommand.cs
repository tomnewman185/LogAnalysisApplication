using System;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public class CancelCommand : ICommand
    {
        readonly CancellationTokenSource _cancellationTokenSource;

        public CancelCommand(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
            {
                throw new ArgumentNullException("cancellationTokenSource");
            }

            _cancellationTokenSource = cancellationTokenSource;
        }

        #region ICommand Implementation
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_cancellationTokenSource.IsCancellationRequested;
        }

        public void Execute(object parameter)
        {
            _cancellationTokenSource.Cancel();

            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }

            CommandManager.InvalidateRequerySuggested();
        }
        #endregion
    }
}

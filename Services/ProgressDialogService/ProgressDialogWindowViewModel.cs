using System;
using System.Linq;
using System.Threading;
using DevExpress.Mvvm;

namespace LogAnalysisTool.Services.ProgressDialogService
{
    public class ProgressDialogWindowViewModel : ViewModelBase
    {
        private int _issueCount;
        private int _nonIssueCount;
        private int _percentageDone;
        private int _percentageIssues;
        private int _percentageNonIssues;
        private int _remainingPercentageDone;
        private bool _showAnomalyProgressBar;
        private string _title;
        private readonly CancellationTokenRegistration _tokenRegistration;

        public ProgressDialogWindowViewModel(string title, CancellationToken cancellationToken, CancelCommand cancelCommand)
        {
            Title = title;
            CancelCommand = cancelCommand;
            _tokenRegistration = cancellationToken.Register(OnCancelled);
            Progress = new Progress<ProgressReportInfo>(OnProgress);
        }

        private void OnCancelled()
        {
            _tokenRegistration.Unregister();
            Close();
        }

        private void OnProgress(ProgressReportInfo progressInfo)
        {
            PercentageDone = progressInfo.PercentageDone;
            ShowAnomalyProgressBar = progressInfo.ShowAnomalyProgressBar;
        }

        public void Close()
        {
            CurrentWindowService.Close();
        }

        public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

        public bool IsCancellable => CancelCommand != null;

        public int IssueCount
        {
            get { return _issueCount; }
            set
            {
                if (_issueCount == value)
                {
                    return;
                }

                _issueCount = value;
                RaisePropertyChanged(nameof(IssueCount));
            }
        }

        public int NonIssueCount
        {
            get { return _nonIssueCount; }
            set
            {
                if (_nonIssueCount == value)
                {
                    return;
                }

                _nonIssueCount = value;
                RaisePropertyChanged(nameof(NonIssueCount));
            }
        }

        public int PercentageDone
        {
            get { return _percentageDone; }
            set
            {
                if (_percentageDone == value)
                {
                    return;
                }

                _percentageDone = value;
                RaisePropertyChanged(nameof(PercentageDone));
                RemainingPercentageDone = 100 - PercentageDone;
            }
        }

        public int PercentageIssues
        {
            get { return _percentageIssues; }
            set
            {
                if (_percentageIssues == value)
                {
                    return;
                }

                _percentageIssues = value;
                RaisePropertyChanged(nameof(PercentageIssues));
            }
        }

        public int PercentageNonIssues
        {
            get { return _percentageNonIssues; }
            set
            {
                if (_percentageNonIssues == value)
                {
                    return;
                }

                _percentageNonIssues = value;
                RaisePropertyChanged(nameof(PercentageNonIssues));
            }
        }

        public IProgress<ProgressReportInfo> Progress { get; private set; }

        public int RemainingPercentageDone
        {
            get { return _remainingPercentageDone; }
            set
            {
                if (_remainingPercentageDone == value)
                {
                    return;
                }

                _remainingPercentageDone = value;
                RaisePropertyChanged(nameof(RemainingPercentageDone));
            }
        }

        public bool ShowAnomalyProgressBar
        {
            get { return _showAnomalyProgressBar; }
            set
            {
                if (_showAnomalyProgressBar == value)
                {
                    return;
                }

                _showAnomalyProgressBar = value;
                RaisePropertyChanged(nameof(ShowAnomalyProgressBar));
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public CancelCommand CancelCommand { get; private set; }
    }
}

using System;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using LogAnalysisTool.Messages;

namespace LogAnalysisTool.ViewModels
{
    /// <summary>
    /// MainWindowViewModel - Functionality pertaining to the main window of the application
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private bool _statisticsEnabled;

        public MainWindowViewModel()
        {
            AnalysisViewModel = new AnalysisViewModel();
            StatisticsViewModel = new StatisticsViewModel();

            Messenger.Default.Register<AnalysisResultsAvailableMessage>(this, AnalysisResultsAvailable);
            Messenger.Default.Register<AnalysisResultsNotAvailableMessage>(this, AnalysisNotResultsAvailable);

            // Sets the current view model of the application to the analysis view model
            CurrentViewModel = AnalysisViewModel;
        }

        /// <summary>
        /// AnalysisResultsNotAvailable() - method to determine if there are analysis results available, which
        /// determines whether or not the statisitcs tab is available for selection
        /// </summary>
        /// <param name="message"></param>
        private void AnalysisNotResultsAvailable(AnalysisResultsNotAvailableMessage message)
        {
            StatisticsEnabled = false;
        }

        /// <summary>
        /// /// AnalysisResultsAvailable() - method to determine if there are analysis results available, which
        /// determines whether or not the statisitcs tab is available for selection
        /// </summary>
        /// <param name="message"></param>
        private void AnalysisResultsAvailable(AnalysisResultsAvailableMessage message)
        {
            StatisticsEnabled = true;
        }

        /// <summary>
        /// ShowAnalysis() - changes the view model to the analysis view model
        /// </summary>
        private void ShowAnalysis()
        {
            CurrentViewModel = AnalysisViewModel;
        }

        /// <summary>
        /// ShowStatistics() - changes the view model to the statistics view model
        /// </summary>
        private void ShowStatistics()
        {
            CurrentViewModel = StatisticsViewModel;
        }

        private AnalysisViewModel AnalysisViewModel { get; set; }

        private StatisticsViewModel StatisticsViewModel { get; set; }

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel == value)
                {
                    return;
                }

                _currentViewModel = value;
                RaisePropertyChanged(nameof(CurrentViewModel));
            }
        }

        public bool StatisticsEnabled
        {
            get { return _statisticsEnabled; }
            set
            {
                if (_statisticsEnabled == value)
                {
                    return;
                }

                _statisticsEnabled = value;
                RaisePropertyChanged(nameof(StatisticsEnabled));
            }
        }

        private DelegateCommand _showAnalysisCommand;

        private DelegateCommand _showStatsCommand;

        public ICommand ShowAnalysisCommand => _showAnalysisCommand ??= new DelegateCommand(ShowAnalysis);

        public ICommand ShowStatsCommand => _showStatsCommand ??= new DelegateCommand(ShowStatistics);
    }
}

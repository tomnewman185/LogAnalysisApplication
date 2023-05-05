using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using LogAnalysisTool.Components.Charts;
using LogAnalysisTool.Detections.AnomalyBasedDetectionTests;
using LogAnalysisTool.Detections.BehaviourBasedDetectionTests;
using LogAnalysisTool.Messages;
using LogAnalysisTool.StatisticReporting;

namespace LogAnalysisTool.ViewModels
{
    // Statistics View Model - functionality pertaining to the statistics portion of the application
    internal class StatisticsViewModel : ViewModelBase
    {
        private string _fileName;
        private int _totalEntryCount;

        public StatisticsViewModel()
        {
            // Creating new instances of the collections for the statistics context
            VulnerabilityPieSlices = new ObservableCollection<VulnerabilitySlicePieViewModel>();
            Vulnerabilities = new ObservableCollection<MaliciousLogEntryInfo>();
            MLVulnerabilities = new ObservableCollection<IAnomalyDetectionItem>();

            // Listen for when a new analysis has been run and call the UpdateStats() method
            Messenger.Default.Register<AnalysisResultsAvailableMessage>(this, UpdateStats);
        }

        private void UpdateStats(AnalysisResultsAvailableMessage message)
        {
            // Clear the collections from any previous runs
            Vulnerabilities.Clear();
            MLVulnerabilities.Clear();
            VulnerabilityPieSlices.Clear();

            // Set FileName
            FileName = message.FileName;

            // Populate collections
            Vulnerabilities = new ObservableCollection<MaliciousLogEntryInfo>(message.Vulnerabilities);
            MLVulnerabilities = new ObservableCollection<IAnomalyDetectionItem>(message.MLVulnerabilities);

            // Set log line count
            TotalEntryCount = message.TotalLogLineCount;

            // Set Pie Slices collection
            IEnumerable<(string Label, double Percentage, int Count)>? pieData = StatisticsHelper.GetPieChartStats(message.Vulnerabilities);

            int index = 0;
            var acc = 0.0;
            foreach (var (Label, Percentage, Count) in pieData.OrderBy(p => p.Percentage))
            {
                VulnerabilityPieSlices.Add(new VulnerabilitySlicePieViewModel()
                                           {
                                               Name = Label,
                                               Percentage = Percentage,
                                               Accumulated = acc,
                                               Count = Count,
                                               Index = index
                                           });
                acc += Percentage;
                index++;
            }
        }

        // Pulls name of file
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value)
                {
                    return;
                }

                _fileName = value;
                RaisePropertyChanged(nameof(FileName));
            }
        }

        // Properties pulling the information needed for the statistics
        public string LeastCommonVulnerabilityDetected => Vulnerabilities.GetMinLogIssueInfo().testName;

        public int MaxVulnerabilityCount => Vulnerabilities.GetMaxLogIssueInfo().maxCount;

        public int MinVulnerabilityCount => Vulnerabilities.GetMinLogIssueInfo().minCount;

        public ObservableCollection<IAnomalyDetectionItem> MLVulnerabilities { get; set; }

        public string MostCommonTestDetected => Vulnerabilities.GetMaxLogIssueInfo().testName;

        public double PercentageOfEntriesVulnerable => Math.Round(((double)TotalVulnerabilityCount / TotalEntryCount) * 100);

        public int TotalEntryCount
        {
            get { return _totalEntryCount; }
            set
            {
                if (_totalEntryCount == value)
                {
                    return;
                }

                _totalEntryCount = value;
                RaisePropertyChanged(nameof(TotalEntryCount));
            }
        }

        public int TotalVulnerabilityCount => Vulnerabilities.Count + MLVulnerabilities.Count;

        public ObservableCollection<MaliciousLogEntryInfo> Vulnerabilities { get; set; }

        public ObservableCollection<VulnerabilitySlicePieViewModel> VulnerabilityPieSlices { get; set; }
    }
}

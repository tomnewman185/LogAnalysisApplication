using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using LogAnalysisTool.ApacheLogs;
using LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests;
using LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests.MachineLearning;
using LogAnalysisTool.ApacheLogs.AnomalyBasedDetectionTests.MachineLearning.Training;
using LogAnalysisTool.ApacheLogs.BehaviourBasedDetectionTests;
using LogAnalysisTool.Messages;
using LogAnalysisTool.Services.ProgressDialogService;
using Microsoft.ML;

namespace LogAnalysisTool.ViewModels
{
    // Analysis View Model - functionality pertaining to the analysis portion of the application
    internal class AnalysisViewModel : ViewModelBase
    {
        private string _fileName;
        private LogType _logType;
        private bool _runAnalysisCompleted;
        private bool _runningAnalysis;
        private ApacheLogFile _selectedAnomalyItem;
        private MaliciousLogEntryInfo _selectedBehaviouralItem;
        private IDetectionItem _selectedCommonItem;

        private const string _trainingModelFileName = "model.zip";

        public AnalysisViewModel()
        {
            _browseCommand = new DelegateCommand(FileBrowser);
            _runAnalysisCommand = new DelegateCommand(RunAnalysis);
            Vulnerabilities = new ObservableCollection<MaliciousLogEntryInfo>();
            MLVulnerabilities = new ObservableCollection<IAnomalyDetectionItem>();
            CommonVulnerabilities = new ObservableCollection<CommonVulnerability>();
        }

        /// <summary>
        /// FileBrowser() - Opens file dialog box and stores name of file under FileName
        /// </summary>
        private void FileBrowser()
        {
            _ = OpenFileDialogService.ShowDialog();
            FileName = OpenFileDialogService.File.GetFullName();
        }

        /// <summary>
        /// GetPercentageDetections() - Returns the percentage of issues detected
        /// </summary>
        /// <param name="issueCount"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private static int GetPercentageDetections(int issueCount, int rowCount)
        {
            return (int)(issueCount / (double)rowCount);
        }

        /// <summary>
        /// GetPercentageProcessed() - Returns the percentage of the file that has been processed
        /// </summary>
        /// <param name="rowsProcessed"></param>
        /// <param name="totalRowCount"></param>
        /// <returns></returns>
        private static int GetPercentageProcessed(int rowsProcessed, int totalRowCount)
        {
            int percentage;
            if (totalRowCount < 100)
            {
                percentage = (int)(rowsProcessed / (double)totalRowCount);
            }
            else
            {
                // Cast ints to double to avoid integer arithmetic
                double step = totalRowCount / 100.0;

                percentage = (int)(rowsProcessed / step);
            }

            return percentage;
        }

        /// <summary>
        /// IsOKToRun() - Performs checks to see if the correct fields have been populated in order to run the analysis,
        /// otherwise returns an error to the user
        /// </summary>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        private bool IsOKToRun(out string errMessage)
        {
            errMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(FileName))
            {
                errMessage = "Please enter a filename to process";
                return false;
            }

            if (!File.Exists(FileName))
            {
                errMessage = $"The file {FileName} doesn't exist";
                return false;
            }

            return true;
        }

        /// <summary>
        /// ModelTraining() - Method to train the machine learning model
        /// </summary>
        private void ModelTraining()
        {
            // Create new machine learning context
            MLContext context = new();

            // Define file paths for training data & labels
            string dataFileName = @"C:\Users\tomjo\Desktop\AIT-LDS-v1_1\data\mail.cup.com\apache2\mail.cup.com-access.log";
            string labelFileName = @"C:\Users\tomjo\Desktop\AIT-LDS-v1_1\labels\mail.cup.com\apache2\mail.cup.com-access.log";

            // Define pattern for Apache log regular expression
            string pattern = new ApacheLogComponents().LogComponentsRegularExpression;
            Regex regex = new(pattern);

            // Get Training Data
            var dataView = context.Data
                                  .LoadFromEnumerable<ApacheLogFileLabels>(
                    TrainingDataLogReader.GetData(dataFileName, labelFileName, regex));

            // Create pipelines and trainers
            var dataProcessPipeline = context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "Request");

            var trainer = context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

            var trainingDataPipeline = dataProcessPipeline.Append(trainer);

            // Create trained model
            ITransformer trainedModel = trainingDataPipeline.Fit(dataView);

            // Prepare training data for model
            var testingDataView = context.Data
                                         .LoadFromEnumerable<ApacheLogFileLabels>(
                    TrainingDataLogReader.GetData(dataFileName, labelFileName, regex));

            var predictions = trainedModel.Transform(testingDataView);

            var metrics = context.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label", scoreColumnName: "Score");

            // Gather training results
            var results = context.Data.CreateEnumerable<Result>(predictions, reuseRowObject: false).ToList();

            // Save model
            context.Model.Save(trainedModel, testingDataView.Schema, _trainingModelFileName);
        }

        /// <summary>
        /// Prediction() - Logic for the prediction engine used to make predictions on incoming data using the trained
        /// machine learning model
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void Prediction(CancellationToken cancellationToken)
        {
            // Create new machine learning context
            MLContext mlContext = new();

            // Load trained model
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load(_trainingModelFileName, out predictionPipelineSchema);

            // Define pattern for Apache log regular expression
            string pattern = new ApacheLogComponents().LogComponentsRegularExpression;
            Regex regex = new(pattern, RegexOptions.IgnorePatternWhitespace);

            // Gather regular expression groups for user data
            static ApacheLogFile GetDataItem(Match m, int lineCounter)
            {
                var data = new ApacheLogFile()
                           {
                               RemoteHost = m.Groups[ApacheLogComponents.RegexComponentGroups.RemoteHost].Value,
                               RFC931 = m.Groups[ApacheLogComponents.RegexComponentGroups.RFC931].Value,
                               AuthUser = m.Groups[ApacheLogComponents.RegexComponentGroups.AuthUser].Value,
                               Date = m.Groups[ApacheLogComponents.RegexComponentGroups.Date].Value,
                               Request = m.Groups[ApacheLogComponents.RegexComponentGroups.Request].Value,
                               Status = m.Groups[ApacheLogComponents.RegexComponentGroups.Status].Value,
                               Bytes = m.Groups[ApacheLogComponents.RegexComponentGroups.Bytes].Value,
                               Referer = m.Groups[ApacheLogComponents.RegexComponentGroups.Referer].Value,
                               UserAgent = m.Groups[ApacheLogComponents.RegexComponentGroups.UserAgent].Value,
                               LineNumber = lineCounter
                           };

                return data;
            }

            // Load user inputted data
            var inputData = mlContext.Data
                                     .LoadFromEnumerable<ApacheLogFile>(
                    MachineLearningDataLogReader<ApacheLogFile>.GetData(FileName, regex, GetDataItem));

            // Create predictions
            IDataView predictions = predictionPipeline.Transform(inputData);

            // Transform predictions into results
            var results = mlContext.Data.CreateEnumerable<Result>(predictions, reuseRowObject: false);

            // If a result is found to be likely to be malicious, add it to the MLVulnerabilities observable object
            foreach (var result in results.Where(r => r.PredictedLabel == true))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Update collection via dispatcher to make sure executed on UI thread. 
                DispatcherService
                    .Invoke(() => MLVulnerabilities.Add(result));
            }
        }

        /// <summary>
        /// ProcessFile() - Logic behind conducting the behavioural-based analysis
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private async Task ProcessFile(CancellationToken cancellationToken, IProgress<ProgressReportInfo> progress, int rowCount)
        {
            // Clear any previous run
            ResetForNewRun();

            // Get the log type info for the selected log type, i.e. Apache
            ILogTypeInfo logTypeInfo = LogTypeFactory.GetLogType(LogType);

            var torExitNodes = await TorHelper.GetTorExitIPAddresses(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                                                                  "TorAddresses.txt"));

            IEnumerable<IBehaviouralDetectionTest> behaviouralTests = logTypeInfo.GetBehaviouralDetectionTests(torExitNodes);

            // Build the regex structure for the chosen Log Type
            var regex = new Regex(logTypeInfo.LogComponentsRegularExpression, RegexOptions.IgnorePatternWhitespace);

            // Conduct behavioural-tests and add detections to collection of detections
            int previousPercentage = 0;
            int rowsProcessed = 0;
            await foreach (var line in LogReader.ReadLinesAsync(FileName).WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                rowsProcessed += 1;
                bool lineHasDetections = false;
                foreach (var detection in LogReader.GetDetections(line, rowsProcessed, regex, behaviouralTests))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    lineHasDetections = true;
                    Vulnerabilities.Add(detection);
                }
                if (!lineHasDetections)
                {
                }

                // Process progress percentage
                var percentage = GetPercentageProcessed(rowsProcessed, rowCount);
                if (percentage != previousPercentage)
                {
                    int percentageIssues = GetPercentageDetections(Vulnerabilities.Count, rowCount);
                    progress.Report(new ProgressReportInfo()
                    {
                        PercentageDone = percentage,
                    });
                }
                previousPercentage = percentage;
            }

            progress.Report(new ProgressReportInfo() { PercentageDone = 100, ShowAnomalyProgressBar = true });

            var localToken = cancellationToken;
            await Task.Run(() => Prediction(localToken), localToken);
        }

        /// <summary>
        /// ResetForNewRun() - Clear and initialise for a new run
        /// </summary>
        private void ResetForNewRun()
        {
            Vulnerabilities.Clear();
            MLVulnerabilities.Clear();
            CommonVulnerabilities.Clear();

            Messenger.Default.Send<AnalysisResultsNotAvailableMessage>(new AnalysisResultsNotAvailableMessage());

            RunAnalysisCompleted = false;
        }

        /// <summary>
        /// RunAnalysis() - Logic behind conducting the analysis
        /// </summary>
        private async void RunAnalysis()
        {
            try
            {
                RunningAnalysis = true;

                // Check if analysis can be run at the current time, if not produce an error message for the user
                if (!IsOKToRun(out string errMessage))
                {
                    MessageBoxService.ShowMessage(errMessage, "Error", MessageButton.OK, MessageIcon.Error);
                }
                else
                {
                    // First parse the file to count how many lines are in it, to help define the progress of the analysis
                    var rowCount = await LogReader.CountLinesAsync(FileName);

                    await ProgressDialogService.ShowDialog<ProgressReportInfo>(ProcessFile, rowCount);

                    // Work out 'Common' detections between both detection methodologies
                    ProcessCommonDetections();

                    Messenger.Default.Send(new AnalysisResultsAvailableMessage(FileName, Vulnerabilities, MLVulnerabilities, rowCount));

                    RunAnalysisCompleted = true;
                }
            }

            // Check for cancellation of analysis
            catch (OperationCanceledException cancelledException)
            {
                MessageBoxService.ShowMessage("Operation Cancelled", "Cancelled", MessageButton.OK, MessageIcon.Stop);
                ResetForNewRun();
            }

            // Catch for errors
            catch (Exception ex)
            {
                MessageBoxService.ShowMessage(ex.Message, "Error", MessageButton.OK, MessageIcon.Error);

                ResetForNewRun();
            }
            finally
            {
                RunningAnalysis = false;
            }
        }

        /// <summary>
        /// ProcessCommonDetections() - Check for common detections between both detection methodologies
        /// </summary>
        public void ProcessCommonDetections()
        {
            // Create dictionary of behaviorual-based vulnerabilities with line number as key
            var bDict = Vulnerabilities.DistinctBy(b => b.LineNumber).ToDictionary(k => k.LineNumber);

            // Create dictionary of anomaly-based vulnerabilities with line number as Key
            var mlDict = MLVulnerabilities.DistinctBy(b => b.LineNumber).ToDictionary(k => k.LineNumber);

            // Takes common entries found in both dictionaries found by comparing line numbers
            var commonLines = bDict.Keys.Intersect(mlDict.Keys);

            // For each common entry, add it to a new collection CommonVulnerabilities
            foreach (var commonLine in commonLines)
            {
                CommonVulnerabilities.Add(new CommonVulnerability(mlDict[commonLine], bDict[commonLine]));
            }
        }

        public IDispatcherService DispatcherService => GetService<IDispatcherService>();

        public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

        public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();

        public IProgressDialogService ProgressDialogService => GetService<IProgressDialogService>();

        public ObservableCollection<CommonVulnerability> CommonVulnerabilities { get; set; }

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

        // Pulls type of log
        public LogType LogType
        {
            get { return _logType; }
            set
            {
                if (_logType == value)
                {
                    return;
                }

                _logType = value;
                RaisePropertyChanged(nameof(LogType));
            }
        }

        public ObservableCollection<IAnomalyDetectionItem> MLVulnerabilities { get; set; }

        // Flag to determine whether the analysis has been completed
        public bool RunAnalysisCompleted
        {
            get { return _runAnalysisCompleted; }
            set
            {
                if (_runAnalysisCompleted == value)
                {
                    return;
                }

                _runAnalysisCompleted = value;
                RaisePropertyChanged(nameof(RunAnalysisCompleted));
            }
        }

        // Flag to enable / disable controls on View while performing analysis
        public bool RunningAnalysis
        {
            get { return _runningAnalysis; }
            set
            {
                if (_runningAnalysis == value)
                {
                    return;
                }

                _runningAnalysis = value;
                RaisePropertyChanged(nameof(RunningAnalysis));
            }
        }

        // Pulls currently selected item within the anomaly-detection tab
        public ApacheLogFile SelectedAnomalyItem
        {
            get { return _selectedAnomalyItem; }
            set
            {
                if (_selectedAnomalyItem == value)
                {
                    return;
                }

                _selectedAnomalyItem = value;
                RaisePropertyChanged(nameof(SelectedAnomalyItem));
            }
        }

        // Pulls currently selected item within the behavioural-based detection tab
        public MaliciousLogEntryInfo SelectedBehaviouralItem
        {
            get { return _selectedBehaviouralItem; }
            set
            {
                if (_selectedBehaviouralItem == value)
                {
                    return;
                }

                _selectedBehaviouralItem = value;
                RaisePropertyChanged(nameof(SelectedBehaviouralItem));
            }
        }

        // Pulls currently selected item within the common tab
        public IDetectionItem SelectedCommonItem
        {
            get { return _selectedCommonItem; }
            set
            {
                if (_selectedCommonItem == value)
                {
                    return;
                }

                _selectedCommonItem = value;
                RaisePropertyChanged(nameof(SelectedCommonItem));
            }
        }

        public ObservableCollection<MaliciousLogEntryInfo> Vulnerabilities { get; set; }

        private DelegateCommand _browseCommand;

        private readonly DelegateCommand _runAnalysisCommand;

        private readonly DelegateCommand _trainingCommand;

        public ICommand BrowseCommand => _browseCommand ??= new DelegateCommand(FileBrowser);

        public ICommand RunAnalysisCommand => _runAnalysisCommand;
    }
}
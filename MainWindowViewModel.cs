using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

namespace LogAnalysisTool
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private DelegateCommand _browseCommand;

        private DelegateCommand _runAnalysisCommand;
        
        private string _fileName;

        private LogType _logType;

        public ObservableCollection<MaliciousLogEntryInfo> Vulnerabilities { get; set; }
        public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
        public ICommand BrowseCommand => _browseCommand ??= new DelegateCommand(FileBrowser);
        public ICommand RunAnalysisCommand => _runAnalysisCommand;

        public MainWindowViewModel()
        {
            _browseCommand = new DelegateCommand(FileBrowser);
            _runAnalysisCommand = new DelegateCommand(RunAnalysis);
            Vulnerabilities = new ObservableCollection<MaliciousLogEntryInfo>();
        }

        //Pulls name of file
        public string FileName
        {
            get => _fileName;
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

        //Pulls type of log
        public LogType LogType
        {
            get => _logType;
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

        //Opens file dialog box and stores name of file under FileName
        private void FileBrowser()
        {
            _ = OpenFileDialogService.ShowDialog();
            FileName = OpenFileDialogService.File.GetFullName();
        }

        //Adds each vulnerability detected to the collection of vulnerabilities 
        private void RunAnalysis()
        {
            Vulnerabilities.Clear();

            foreach (var issue in LogAnalysisTool.LogReader.Read(FileName, LogType))
            {
                Vulnerabilities.Add(issue);
            }
        }
    }
}

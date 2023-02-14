using System;
using System.Collections.Generic;
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
        
        private string _fileName;
        public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
        public ICommand BrowseCommand => _browseCommand ??= new DelegateCommand(FileBrowser);

        public MainWindowViewModel()
        {
            _browseCommand = new DelegateCommand(FileBrowser);
        }

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
        private void FileBrowser()
        {
            _ = OpenFileDialogService.ShowDialog();
            FileName = OpenFileDialogService.File.GetFullName();
        }
    }
}

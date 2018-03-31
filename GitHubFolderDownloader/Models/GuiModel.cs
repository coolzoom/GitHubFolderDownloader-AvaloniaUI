using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
<<<<<<< HEAD
using Avalonia.Collections;
=======
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.Models
{
    public class GuiModel : INotifyPropertyChanged
    {

        private string _gitHubToken;
        private String _logs;
        private string _outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private double _progressBarValue;
        private string _repositoryFolderFullUrl;
        private string _repositoryName;
        private string _repositoryOwner;
        private string _repositorySubDir;
        private string _selectedBranch = string.Empty;
        private List<string> _branches = new List<string> { "master" };
<<<<<<< HEAD
        private TraceRedirector _traceRedirector;
        private bool _hasStarted;
=======
        private TraceListenerRedirector _traceRedirector;
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545

        public GuiModel()
        {
            GitHubEntries = new ObservableCollection<GitHubEntry>();

<<<<<<< HEAD
            _traceRedirector = new TraceRedirector();
=======
            _traceRedirector = new TraceListenerRedirector();
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
            Trace.Listeners.Add(_traceRedirector);
            _traceRedirector.WriteAction = (_) =>
            {
                Logs += _;
            };
        }

        public ObservableCollection<GitHubEntry> GitHubEntries { set; get; }

        public string GitHubToken
        {
            get { return _gitHubToken; }
            set
            {
                _gitHubToken = value;
                NotifyPropertyChanged(nameof(GitHubToken));
            }
        }

        public String Logs
        {
            get { return _logs; }
            set
            {
                _logs = value;
                NotifyPropertyChanged(nameof(Logs));
            }
        }

        public string OutputPath
        {
            get { return _outputPath; }
            set
            {
                _outputPath = value;
                NotifyPropertyChanged(nameof(OutputPath));
            }
        }

        public double ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                NotifyPropertyChanged(nameof(ProgressBarValue));
            }
        }

        public string RepositoryFolderFullUrl
        {
            get { return _repositoryFolderFullUrl; }
            set
            {
                _repositoryFolderFullUrl = value;
                NotifyPropertyChanged(nameof(RepositoryFolderFullUrl));
<<<<<<< HEAD
            }
        }

        public bool HasStarted
        {
            get { return _hasStarted; }
            set
            {
                _hasStarted = value;
                NotifyPropertyChanged(nameof(HasStarted));
=======
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
            }
        }

        public string RepositoryName
        {
            get { return _repositoryName; }
            set
            {
                if (value == null) value = string.Empty;
                _repositoryName = value.Trim('/');
                NotifyPropertyChanged(nameof(RepositoryName));
            }
        }

        public string RepositoryOwner
        {
            get { return _repositoryOwner; }
            set
            {
                if (value == null) value = string.Empty;
                _repositoryOwner = value.Trim('/');
                NotifyPropertyChanged(nameof(RepositoryOwner));
            }
        }

        public string RepositorySubDir
        {
            get { return _repositorySubDir; }
            set
            {
                if (value == null) value = string.Empty;
                _repositorySubDir = value.Trim('/');
                NotifyPropertyChanged(nameof(RepositorySubDir));
            }
        }

        public string SelectedBranch
        {
            get { return _selectedBranch; }
            set
            {
                if (value == null) value = string.Empty;
                _selectedBranch = value.Trim('/');
                NotifyPropertyChanged(nameof(SelectedBranch));
            }
        }

        public List<string> Branches
        {
            get { return _branches; }
            set
            {
                _branches = value;
                NotifyPropertyChanged(nameof(Branches));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
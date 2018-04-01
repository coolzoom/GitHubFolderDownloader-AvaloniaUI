using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.Models
{
    public class GuiModel : INotifyPropertyChanged
    {
        public GuiModel()
        {
            var _traceRedirector = new TraceRedirector();
            Trace.Listeners.Add(_traceRedirector);
            _traceRedirector.WriteAction = _ => Logs += _;
        }

        public ObservableCollection<GitHubEntry> GitHubEntries { set; get; } = new ObservableCollection<GitHubEntry>();

        public string GitHubToken { get; set; }

        public string Logs { get; set; }

        public string OutputPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public double ProgressBarValue { get; set; }
        public string RepositoryFolderFullUrl { get; set; }

        public bool HasStarted { get; set; }
        public bool IsValidRepoURL { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositorySubDir { get; set; }
        public string SelectedBranch { get; set; }

        public ObservableCollection<string> Branches { get; set; } = new ObservableCollection<string>();

        #pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
        #pragma warning restore CS0067

    }
}
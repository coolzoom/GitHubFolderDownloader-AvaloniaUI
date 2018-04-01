using System;
using System.ComponentModel;
using System.Windows;
using GitHubFolderDownloader.Core;
using GitHubFolderDownloader.Models;
using GitHubFolderDownloader.Toolkit;
using Avalonia.Reactive;
using ReactiveUI;
using Avalonia;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Reflection;

namespace GitHubFolderDownloader.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly GitHubDownloader _gitHubDownloader;

        public MainWindowViewModel()
        {

            GuiState = new GuiModel
            {
                GitHubToken = PersistentConfig.GetConfigData("GitHubToken")
            };

            GuiState.PropertyChanged += PropertyChanged;

            _gitHubDownloader = new GitHubDownloader(GuiState)
            {
                Finished = url =>
                {
                    Trace.WriteLine($"Finished {url}.", "Info");
                    DispatcherHelper.DispatchAction(() => GuiState.HasStarted = false);
                }
            };

            StartCommand = ReactiveCommand.Create(() => StartDownload(), CanStartDownloadExecute);
            StopCommand = ReactiveCommand.Create(() => StopDownload(), CanStopDownloadExecute);
            
        }

        public string Title { get; set; } = $"Github Folder Downloader 2.0.0";

        public GuiModel GuiState { set; get; }

        public ReactiveCommand StartCommand { get; set; }

        public ReactiveCommand StopCommand { set; get; }

        private IObservable<bool> CanStartDownloadExecute =>
            GuiState.WhenAnyValue(x => x.RepositoryName,
                                      x => x.RepositoryOwner,
                                      x => x.OutputPath,
                                      x => x.HasStarted, (a, b, c, d) =>
                                        !string.IsNullOrWhiteSpace(a) &&
                                        !string.IsNullOrWhiteSpace(b) &&
                                        !string.IsNullOrWhiteSpace(c) &&
                                        !d);

        private IObservable<bool> CanStopDownloadExecute => 
            GuiState.WhenAnyValue(x => x.HasStarted, 
                                      (a) => a == true);

        public void Exit()
        {
            SaveSettings();
            _gitHubDownloader.Stop();
        }

        private void StartDownload()
        {
            GuiState.HasStarted = true;
            SaveSettings();
            _gitHubDownloader.Start();
        }

        private void StopDownload()
        {
            _gitHubDownloader.Stop();
            GuiState.HasStarted = false;
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RepositoryFolderFullUrl":
                    new ApiUrl(GuiState).SetApiSegments();
                    new GitHubBranchList(GuiState).SetBranchesList();
                    break;
            }
        }
        
        private void SaveSettings()
        {
            if (!string.IsNullOrWhiteSpace(GuiState.GitHubToken))
            {
                PersistentConfig.SetConfigData("GitHubToken", GuiState.GitHubToken);
            }
        }
         
    }
}
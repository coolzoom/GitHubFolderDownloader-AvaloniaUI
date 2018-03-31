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

namespace GitHubFolderDownloader.ViewModels
{
    public class MainWindowViewModel 
    {
        private readonly GitHubDownloader _gitHubDownloader;
        private bool _isStarted;



        public MainWindowViewModel()
        {

            GuiModelData = new GuiModel
            {
                GitHubToken = ConfigSetGet.GetConfigData("GitHubToken")
            };

            GuiModelData.PropertyChanged += PropertyChanged;

            _gitHubDownloader = new GitHubDownloader(GuiModelData)
            {
                Finished = url =>
                {
                    Trace.WriteLine($"Finished {url}.", "Info");
                    _isStarted = false;
                }
            };

            StartCommand = ReactiveCommand.Create(() => StartDownload(), CanDownloadExecute);
            StopCommand = ReactiveCommand.Create(() => StopDownload());

            ManageAppExit();

        }


        public GuiModel GuiModelData { set; get; }

        public ReactiveCommand StartCommand { get; set; }

        public ReactiveCommand StopCommand { set; get; }

        private IObservable<bool> CanDownloadExecute =>
            GuiModelData.WhenAnyValue(x => x.RepositoryName,
                                      x => x.RepositoryOwner,
                                      x => x.OutputPath, (a, b, c) =>
                                        !string.IsNullOrWhiteSpace(a) &&
                                        !string.IsNullOrWhiteSpace(b) &&
                                        !string.IsNullOrWhiteSpace(c));


                                        
        private void ExitApp(object sender, EventArgs e)
        {
            SaveSettings();
            _gitHubDownloader.Stop();
        }


        private void StartDownload()
        {
            _isStarted = true;

            SaveSettings();

            _gitHubDownloader.Start();
        }

        private void StopDownload()
        {
            _gitHubDownloader.Stop();
            _isStarted = false;
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RepositoryFolderFullUrl":
                    new ApiUrl(GuiModelData).SetApiSegments();
                    new GitHubBranchList(GuiModelData).SetBranchesList();
                    break;
            }
        }

        private void ManageAppExit()
        {
            if (Application.Current == null) return;
            Application.Current.OnExit += ExitApp;
        }

        private void SaveSettings()
        {
            if (!string.IsNullOrWhiteSpace(GuiModelData.GitHubToken))
            {
                ConfigSetGet.SetConfigData("GitHubToken", GuiModelData.GitHubToken);
            }
        }

    }
}
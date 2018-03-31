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

<<<<<<< HEAD
            GuiModelData.PropertyChanged += PropertyChanged;
=======
            GuiModelData.PropertyChanged += guiModelDataPropertyChanged;
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545

            _gitHubDownloader = new GitHubDownloader(GuiModelData)
            {
                Finished = url =>
                {
                    Trace.WriteLine($"Finished {url}.", "Info");
                    _isStarted = false;
                }
            };

<<<<<<< HEAD
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
=======
            StartCommand = ReactiveCommand.Create(() => doStart(), canDoStart);
            StopCommand = ReactiveCommand.Create(() => doStop());

          //  AppMessenger.Messenger.Register<string>("ShowLog", log => addLog(log));
            manageAppExit();

        }

 
        public GuiModel GuiModelData { set; get; }

        public ReactiveCommand StartCommand { get; set; }

        public ReactiveCommand StopCommand { set; get; }
 

        private IObservable<bool> canDoStart =>
        GuiModelData.WhenAnyValue(x => x.RepositoryName,
                                  x => x.RepositoryOwner,
                                  x => x.OutputPath, (a, b, c) =>
                                  {
                                      return !string.IsNullOrWhiteSpace(a) &&
                                      !string.IsNullOrWhiteSpace(b) &&
                                      !string.IsNullOrWhiteSpace(c) &&
                                      !_isStarted;
                                  });

        private void currentExit(object sender, EventArgs e)
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
        {
            SaveSettings();
            _gitHubDownloader.Stop();
        }


<<<<<<< HEAD
        private void StartDownload()
=======
        private void doStart()
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
        {
            _isStarted = true;

            SaveSettings();

            _gitHubDownloader.Start();
        }

<<<<<<< HEAD
        private void StopDownload()
=======
        private void doStop()
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
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
<<<<<<< HEAD
            Application.Current.OnExit += ExitApp;
=======
            Application.Current.OnExit += currentExit;
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
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
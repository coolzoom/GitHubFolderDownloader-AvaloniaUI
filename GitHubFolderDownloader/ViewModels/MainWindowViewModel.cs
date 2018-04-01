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
using Avalonia.Controls;

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

            GuiState.WhenAnyValue(p => p.RepositoryFolderFullUrl)
                    .Throttle(TimeSpan.FromMilliseconds(250))
                    .Subscribe(_ => ValidateAndExecuteURL(_));

            _gitHubDownloader = new GitHubDownloader(GuiState)
            {
                Finished = url =>
                {
                    Trace.WriteLine($"Finished {url}.", "Info");
                    DispatcherHelper.DispatchAction(() => GuiState.HasStarted = false);
                }
            };

            StartCommand = ReactiveCommand.Create(() => StartDownload(), CanStartDownloadExecute);
            StopCommand = ReactiveCommand.Create(() => StopDownload());
            OpenFolderCommand = ReactiveCommand.Create(() => GetSaveFolder());

        }

        private async void GetSaveFolder()
        {
            var dialog = new OpenFolderDialog();
            dialog.Title = "Select directory to save downloads";
            dialog.InitialDirectory = GuiState.OutputPath;
            var k = await dialog.ShowAsync();
            GuiState.OutputPath = k;
        }

        private void ValidateAndExecuteURL(string _)
        {
            bool isValidUrl = Uri.TryCreate(_, UriKind.Absolute, out var target);

            if (isValidUrl)
            {
                isValidUrl &= (target.Scheme == Uri.UriSchemeHttp || target.Scheme == Uri.UriSchemeHttps);
                isValidUrl &= (target.Host == "github.com");
            }

            if (isValidUrl)
            {
                try
                {
                    DispatcherHelper.DispatchAction(() =>
                    {
                        new ApiUrl(GuiState).SetApiSegments();
                        new GitHubBranchList(GuiState).SetBranchesList(true);
                        GuiState.IsValidRepoURL = true;
                    });

                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message, "Error");
                }
            }

            DispatcherHelper.DispatchAction(() => GuiState.IsValidRepoURL = false);

        }

        public string Title { get; set; } = $"Github Folder Downloader 2.0.0";

        public GuiModel GuiState { set; get; }
        public ReactiveCommand StartCommand { get; set; }

        public ReactiveCommand StopCommand { set; get; }

        public ReactiveCommand OpenFolderCommand { set; get; }

        private IObservable<bool> CanStartDownloadExecute =>
            GuiState.WhenAnyValue(x => x.RepositoryName,
                                      x => x.RepositoryOwner,
                                      x => x.OutputPath,
                                      x => x.HasStarted, (a, b, c, d) =>
                                        !string.IsNullOrWhiteSpace(a) &&
                                        !string.IsNullOrWhiteSpace(b) &&
                                        !string.IsNullOrWhiteSpace(c) &&
                                        !d);

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

        private void SaveSettings()
        {
            if (!string.IsNullOrWhiteSpace(GuiState.GitHubToken))
            {
                PersistentConfig.SetConfigData("GitHubToken", GuiState.GitHubToken);
            }
        }

    }
}
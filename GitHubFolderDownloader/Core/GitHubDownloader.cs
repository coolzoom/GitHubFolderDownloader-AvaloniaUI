﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GitHubFolderDownloader.Models;
using GitHubFolderDownloader.Toolkit;
using Newtonsoft.Json;

namespace GitHubFolderDownloader.Core
{
    public class GitHubDownloader
    {
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly GuiModel _guiModelData;
        private readonly ParallelTasksQueue _parallelTasksQueue = new ParallelTasksQueue(Environment.ProcessorCount);

        public GitHubDownloader(GuiModel guiModelData)
        {
            _guiModelData = guiModelData;
        }

        public Action<string> Finished { set; get; }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {

                DispatcherHelper.DispatchAction(
                    () => _guiModelData.GitHubEntries.Clear());
                if (!NetworkStatus.IsConnectedToInternet())
                {
                    Trace.WriteLine("Internet connection is not avalaible.", "Error");
                    return;
                }

                var entries = getGitHubEntries(_guiModelData.RepositorySubDir, _guiModelData.SelectedBranch);
                if (!entries.Any())
                {
                    Trace.WriteLine("The folder is empty.", "Error");
                    return;
                }

                var baseFolder = getBaseFolder();
                processListOfEntries(entries, baseFolder);
            }).ContinueWith(obj => Finished(getApiRootUrl()));
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        private static string getOutFolder(GitHubEntry localItem, string baseFoler)
        {
            var pathDir = Path.GetDirectoryName(localItem.Path);
            var outFolder = Path.Combine(baseFoler, pathDir);
            if (Directory.Exists(outFolder))
            {
                return outFolder;
            }

            var info = Directory.CreateDirectory(outFolder);
            return info.FullName;
        }

        private string getApiRootUrl()
        {
            return new ApiUrl(_guiModelData).GetApiUrl(_guiModelData.RepositorySubDir, _guiModelData.SelectedBranch);
        }

        private string getBaseFolder()
        {
            var baseFoler = _guiModelData.OutputPath;
            if (!Directory.Exists(baseFoler))
            {
                Directory.CreateDirectory(baseFoler);
            }
            return baseFoler;
        }

        private Action getDownloadTask(GitHubEntry localItem, string outFolder)
        {
            Action action = () =>
            {
                try
                {
                    Downloader.DownloadFile(
                        url: localItem.DownloadUrl,
                        outFolder: outFolder,
                        expectedFileSize: localItem.Size,
                        cancellationToken: _cancellationToken.Token,
                        onPercentChange: downloadPercent =>
                        {
                            localItem.DownloadPercent = downloadPercent;
                            // if (downloadPercent == 100)
                            // {
                            //     DispatcherHelper.DispatchAction(
                            //         () => _guiModelData.GitHubEntries.Remove(localItem));
                            // }
                        });
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{localItem.DownloadUrl} -> {ex.Message}", "Error");
                }
            };
            return action;
        }

        private GitHubEntry[] getGitHubEntries(string repositorySubDir, string branch)
        {
            var url = new ApiUrl(_guiModelData).GetApiUrl(repositorySubDir, branch);
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", Downloader.UA);
                webClient.Headers.Add("Authorization", string.Format("Token {0}", _guiModelData.GitHubToken));
                var jsonData = webClient.DownloadString(url);
                return JsonConvert.DeserializeObject<GitHubEntry[]>(jsonData);
            }
        }

        private void processListOfEntries(IEnumerable<GitHubEntry> entries, string baseFoler)
        {
            var outFolder = string.Empty;
            try
            {
                var waitingList = new List<Task>();
                foreach (var item in entries)
                {
                    var localItem = item;

                    if (localItem.Type.Equals("dir"))
                    {
                        var subEntries = getGitHubEntries(localItem.Path, _guiModelData.SelectedBranch);
                        if (!subEntries.Any())
                        {
                            continue;
                        }

                        processListOfEntries(subEntries, baseFoler);
                    }
                    else if (localItem.Type.Equals("file"))
                    {
                        DispatcherHelper.DispatchAction(() => _guiModelData.GitHubEntries.Add(localItem));

                        outFolder = getOutFolder(localItem, baseFoler);
                        var action = getDownloadTask(localItem, outFolder);
                        var task = Task.Factory.StartNew(
                            action,
                            _cancellationToken.Token,
                            TaskCreationOptions.None,
                            _parallelTasksQueue);
                        waitingList.Add(task);
                    }
                }
                Task.WaitAll(waitingList.ToArray(), _cancellationToken.Token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message, "Error");
            }
            finally
            {
                Finished(outFolder);
            }
        }
    }
}
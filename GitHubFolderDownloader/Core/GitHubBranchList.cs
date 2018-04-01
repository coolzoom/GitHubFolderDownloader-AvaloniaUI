using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GitHubFolderDownloader.Models;
using GitHubFolderDownloader.Toolkit;
using Newtonsoft.Json;

namespace GitHubFolderDownloader.Core
{
    public class GitHubBranchList
    {
        private readonly GuiModel _GuiState;

        public GitHubBranchList(GuiModel GuiState)
        {
            _GuiState = GuiState;
        }

        public void SetBranchesList()
        {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                if (!NetworkStatus.IsConnectedToInternet())
                {
                    Trace.WriteLine("The internet connection was not found.", "Logs");
                    return null;
                }

                var entries = getGitHubBranches();
                if (!entries.Any())
                {
                    Trace.WriteLine("Failed to list branches.", "Logs");
                    return null;
                }

                return entries;
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                    {
                        task.Exception.Flatten().Handle(ex =>
                        {
                            Trace.WriteLine(ex.Message, "Error");
                            return false;
                        });
                    }
                    return;
                }

                var entries = task.Result;
                _GuiState.Branches = entries?.Select(x => x.Name).ToList();
            }, taskScheduler);
        }

        private GitHubBranch[] getGitHubBranches()
        {
            var url = new ApiUrl(_GuiState).GetBranchesApiUrl();
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", Downloader.UA);
                webClient.Headers.Add("Authorization", string.Format("Token {0}", _GuiState));
                var jsonData = webClient.DownloadString(url);
                return JsonConvert.DeserializeObject<GitHubBranch[]>(jsonData);
            }
        }
    }
}
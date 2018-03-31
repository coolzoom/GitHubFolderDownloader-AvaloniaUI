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
        private readonly GuiModel _guiModelData;

        public GitHubBranchList(GuiModel guiModelData)
        {
            _guiModelData = guiModelData;
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
<<<<<<< HEAD
                            Trace.WriteLine(ex.Message, "Error");
=======
                            Trace.WriteLine(ex.Message, "Errors");
>>>>>>> 2d333c347ab4bd8ae79a1c147d7204502bc6d545
                            return false;
                        });
                    }
                    return;
                }

                var entries = task.Result;
                _guiModelData.Branches = entries?.Select(x => x.Name).ToList();
            }, taskScheduler);
        }

        private GitHubBranch[] getGitHubBranches()
        {
            var url = new ApiUrl(_guiModelData).GetBranchesApiUrl();
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", Downloader.UA);
                webClient.Headers.Add("Authorization", string.Format("Token {0}", _guiModelData));
                var jsonData = webClient.DownloadString(url);
                return JsonConvert.DeserializeObject<GitHubBranch[]>(jsonData);
            }
        }
    }
}
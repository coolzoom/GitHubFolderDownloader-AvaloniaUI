using System;
using System.Linq;
using System.Text;
using GitHubFolderDownloader.Models;

namespace GitHubFolderDownloader.Core
{
    public class ApiUrl
    {
        private readonly GuiModel _GuiState;

        public ApiUrl(GuiModel GuiState)
        {
            _GuiState = GuiState;
        }

        public string GetApiUrl(string repositorySubDir, string branch)
        {
            /*
            This API has an upper limit of 1,000 files for a directory.
            If you need to retrieve more files, use the Git Trees API.
            This API supports files up to 1 megabyte in size.
            */

            if (repositorySubDir == null)
            {
                repositorySubDir = string.Empty;
            }

            var branchName = string.Empty;
            if(!string.IsNullOrWhiteSpace(branch))
            {
                branchName = string.Format("?ref={0}", branch);
            }

            var url = string.Format("https://api.github.com/repos/{0}/{1}/contents/{2}{3}",
                Uri.EscapeUriString(_GuiState.RepositoryOwner),
                Uri.EscapeUriString(_GuiState.RepositoryName),
                Uri.EscapeUriString(repositorySubDir),
                branchName);
            return url;
        }

        public void SetApiSegments()
        { 
                var uri = new Uri(_GuiState.RepositoryFolderFullUrl);
                _GuiState.RepositoryOwner = Uri.UnescapeDataString(uri.Segments[1]).Trim('/').Trim('\\');
                _GuiState.RepositoryName = Uri.UnescapeDataString(uri.Segments[2]).Trim('/').Trim('\\');

                var segments = new StringBuilder();
                foreach (var segment in uri.Segments.Skip(5))
                {
                    segments.Append(segment);
                }
                _GuiState.RepositorySubDir = Uri.UnescapeDataString(segments.ToString()).Trim('/').Trim('\\');
     
        }

        public string GetBranchesApiUrl()
        {
            var url = string.Format("https://api.github.com/repos/{0}/{1}/branches",
                Uri.EscapeUriString(_GuiState.RepositoryOwner),
                Uri.EscapeUriString(_GuiState.RepositoryName));

            return url;
        }
    }
}
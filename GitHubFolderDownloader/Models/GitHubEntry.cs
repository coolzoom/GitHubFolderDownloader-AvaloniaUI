using System.ComponentModel;
using Newtonsoft.Json;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.Models
{
    public class GitHubEntry : INotifyPropertyChanged
    {
        public string ConvertedSize => Size.ToBytesReadable();
        public int DownloadPercent { get; set; }

        [JsonProperty(PropertyName = "download_url")]
        public string DownloadUrl { set; get; }

        [JsonProperty(PropertyName = "path")]
        public string Path { set; get; }

        [JsonProperty(PropertyName = "size")]
        public long Size { set; get; }

        [JsonProperty(PropertyName = "type")]
        public string Type { set; get; }

#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

    }
}
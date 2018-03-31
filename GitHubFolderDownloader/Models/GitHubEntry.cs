using System.ComponentModel;
using Newtonsoft.Json;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.Models
{
    public class GitHubEntry : INotifyPropertyChanged
    {
        private int _downloadPercent;

        public string ConvertedSize => Size.ToBytesReadable();

        public int DownloadPercent
        {
            set
            {
                if (_downloadPercent == value) return;
                _downloadPercent = value;
                NotifyPropertyChanged(nameof(DownloadPercent));
            }
            get { return _downloadPercent; }
        }

        [JsonProperty(PropertyName = "download_url")]
        public string DownloadUrl { set; get; }

        [JsonProperty(PropertyName = "path")]
        public string Path { set; get; }

        [JsonProperty(PropertyName = "size")]
        public long Size { set; get; }

        [JsonProperty(PropertyName = "type")]
        public string Type { set; get; }

        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
using LiteDB;
using Newtonsoft.Json;

namespace GitHubFolderDownloader.Models
{
    public class AppSetting
    {
        [BsonId]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
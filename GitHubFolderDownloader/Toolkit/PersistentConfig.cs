using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using GitHubFolderDownloader.Models;
using LiteDB;

namespace GitHubFolderDownloader.Toolkit
{
    public static class PersistentConfig
    {
        public const string AppSettingsDBName = "GitFldrSettings.db";

        /// <summary>
        /// Retrieves application settings stored in a
        /// LiteDB database.    
        /// </summary>
        public static string GetConfigData(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            try
            {
                using (var db = new LiteRepository(AppSettingsDBName))
                {
                    return db.Query<AppSetting>().Where(p => p.Key == key).Single().Value;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error occurred on retrieving {key} : {e.Message}", "Warning");
                return "";
            }
        }

        /// <summary>
        /// Stores a Key-Value pair to the 
        /// persistence database.
        /// </summary>
        public static void SetConfigData(string key, string data)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            using (var db = new LiteRepository(AppSettingsDBName))
            {
                if (db.Query<AppSetting>().Where(p => p.Key == key).Exists())
                {
                    var existing = db.Query<AppSetting>().Where(p => p.Key == key).First();
                    existing.Value = data;
                    db.Update<AppSetting>(existing);
                }
                else
                {
                    db.Insert(new AppSetting()
                    {
                        Key = key,
                        Value = data
                    });
                }
            } 
        }
    }
}
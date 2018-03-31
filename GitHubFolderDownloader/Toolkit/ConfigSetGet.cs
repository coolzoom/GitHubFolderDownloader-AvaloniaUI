using System;
using System.Configuration;
using System.Linq;
using LiteDB;

namespace GitHubFolderDownloader.Toolkit
{
    public static class ConfigSetGet
    {
        public const string AppSettingsDBName = "GitFldrSettings.db";
        public class AppSetting
        {
            [BsonId]
            public string Key { get; set; }
            public string Value { get; set; }
        }
        /// <summary>
        /// read settings from app.config/web.config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException">Could not retrieve a <see cref="T:System.Collections.Specialized.NameValueCollection" /> object with the application settings data.</exception>
        /// <exception cref="InvalidOperationException">Undefined key in app.config/web.config.</exception>
        public static string GetConfigData(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            // //don't load on design time
            // if (Designer.IsInDesignModeStatic)
            //     return "0";

            try
            {
                using (var db = new LiteRepository(AppSettingsDBName))
                {
                    return db.Query<AppSetting>().Where(p => p.Key == key).Single().Value;
                }
            }
            catch (Exception)
            {
                return "";
            }


            // if (!ConfigurationManager.AppSettings.AllKeys.Any(keyItem => keyItem.Equals(key)))
            // {
            //     throw new InvalidOperationException(string.Format("Undefined key in app.config/web.config: {0}", key));
            // }

            // return ConfigurationManager.AppSettings[key];
        }

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
            // var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // config.AppSettings.Settings[key].Value = data;
            // config.Save(ConfigurationSaveMode.Modified);
            // ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
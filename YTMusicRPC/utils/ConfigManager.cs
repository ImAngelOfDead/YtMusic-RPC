using System;
using System.IO;
using Newtonsoft.Json;
using YTMusicRPC.Models;

namespace YTMusicRPC.utils
{
    public static class ConfigManager
    {
        public static readonly string ConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "YTMusicRPC", 
            "config.json");

        private static Logger _logger = Logger.Instance;
        public static Config Config { get; private set; } = new Config();

        static ConfigManager()
        {
            if (File.Exists(ConfigFilePath))
            {
                Config = LoadConfig();
                if (Config.AnalyticsEnabled) return;
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilePath) ?? string.Empty);
                Config = SaveTrackHistory.RequestAnalytics();
                SaveConfig(Config);
            }
        }

        public static void Initialize()
        {
            Console.Clear();
        }

        private static Config LoadConfig()
        {
            var configContent = File.ReadAllText(ConfigFilePath);
            return JsonConvert.DeserializeObject<Config>(configContent) ?? new Config();
        }

        public static void SaveConfig(Config config)
        {
            var configContent = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, configContent);
        }
        
    }
}
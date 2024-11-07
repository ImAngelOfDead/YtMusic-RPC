using System;
using System.IO;
using Newtonsoft.Json;
using YTMusicRPC.Models;
using YTMusicRPC.utils;

namespace YTMusicRPC.Services
{
    public class HistoryService
    {
        private readonly string _historyFolderPath;

        public HistoryService()
        {
            string configFolderPath = Path.GetDirectoryName(ConfigManager.ConfigFilePath) ?? string.Empty;
            _historyFolderPath = Path.Combine(configFolderPath, "history");
            
            if (!Directory.Exists(_historyFolderPath))
            {
                Directory.CreateDirectory(_historyFolderPath);
            }
        }

        public void SaveTrackInfo(TrackInfo trackInfo, string trackUrl)
        {
            string date = DateTime.Now.ToString("MM-dd-yy");
            string filePath = Path.Combine(_historyFolderPath, $"{date}.json");
            
            string Url = $"https://music.youtube.com/watch?v={trackUrl}";

            var logEntry = new
            {
                Time = DateTime.Now.ToString("hh:mm:ss"),
                Artist = trackInfo.Artist,
                Track = trackInfo.Track,
                Url = Url
            };

            
            var logEntries = File.Exists(filePath)
                ? JsonConvert.DeserializeObject<List<object>>(File.ReadAllText(filePath)) ?? new List<object>()
                : new List<object>();

            
            logEntries.Add(logEntry);
            string jsonContent = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
        }
    }
}
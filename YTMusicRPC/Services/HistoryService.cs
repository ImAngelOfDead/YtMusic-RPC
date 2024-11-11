using Newtonsoft.Json;
using YTMusicRPC.Models;
using YTMusicRPC.utils;

namespace YTMusicRPC.Services;

public class HistoryService
{
    private readonly static string _historyFolderPath =
        Path.Combine(Path.GetDirectoryName(ConfigManager.ConfigFilePath) ?? string.Empty, "history");

    public readonly static string _filePath = Path.Combine(_historyFolderPath, "history.json");
    private static readonly HistoryService instance = new HistoryService();
    public static HistoryService Instance => instance;

    public HistoryService(){
        HandleFileCreation();
    }

    public void SaveTrackInfo(TrackInfo trackInfo){
        HandleFileCreation();

        var logEntry = InitializeEntry(trackInfo);

        string newEntryJson = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
        string tempFilePath = _filePath + ".tmp";

        using (var writer = new StreamWriter(tempFilePath))
        using (var reader = new StreamReader(_filePath)){
            writer.WriteLine("[");
            writer.WriteLine(newEntryJson + ",");

            string line;
            bool isFirstLine = true;
            while ((line = reader.ReadLine()) != null){
                if (isFirstLine){
                    isFirstLine = false;
                    continue;
                }

                if (line.Trim() == "]")
                    continue;

                writer.WriteLine(line);
            }

            writer.WriteLine("]");
        }

        File.Delete(_filePath);
        File.Move(tempFilePath, _filePath);
    }

    public async Task<List<object>> GetHistory(){
        int maxEntries = 20;
        var historyEntries = new List<object>();

        await using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var streamReader = new StreamReader(fileStream))
        await using (var jsonReader = new JsonTextReader(streamReader)){
            jsonReader.SupportMultipleContent = true;
            var serializer = new JsonSerializer();

            if (jsonReader.Read() && jsonReader.TokenType == JsonToken.StartArray){
                while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray &&
                       historyEntries.Count < maxEntries){
                    var entry = serializer.Deserialize<object>(jsonReader);
                    if (entry != null){
                        historyEntries.Add(entry);
                    }
                }
            }
        }

        return historyEntries;
    }

    private void HandleFileCreation(){
        if (!Directory.Exists(_historyFolderPath)){
            Directory.CreateDirectory(_historyFolderPath);
        }

        if (!File.Exists(_filePath)){
            File.WriteAllText(_filePath, "[]");
        }
    }

    private object InitializeEntry(TrackInfo trackInfo){
        string Url = $"https://music.youtube.com/watch?v={trackInfo.VideoId}";

        var logEntry = new{
            Time = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"),
            Artist = trackInfo.Artist,
            Track = trackInfo.Track,
            Url = Url
        };
        return logEntry;
    }
}
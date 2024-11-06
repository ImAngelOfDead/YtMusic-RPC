namespace YTMusicRPC.utils;

public static class ConfigManager {
    private static readonly string configFilePath = "config.txt";
    private static Logger _logger = Logger.Instance;

    public static bool IsAnalyticsEnabled() {
        if (!File.Exists(configFilePath)) {
            return SaveTrackHistory.RequestAnalytics().AnalyticsEnabled;
        }

        var configContent = File.ReadAllText(configFilePath);
        return configContent.Contains("AnalyticsEnabled=true");
    }

    public static string GetBotToken() {
        return GetConfigValue("BotToken");
    }

    public static string GetChatId() {
        return GetConfigValue("ChatId");
    }

    private static string GetConfigValue(string key) {
        if (!File.Exists(configFilePath)) return null;

        var configLines = File.ReadAllLines(configFilePath);
        
        foreach (var line in configLines) {
            if (line.StartsWith($"{key}=")) {
                return line.Substring($"{key}=".Length).Trim();
            }
        }

        return null;
    }

    private static void SaveConfig(bool isAnalyticsEnabled, string botToken = null, string chatId = null) {
        string configContent = $"AnalyticsEnabled={(isAnalyticsEnabled ? "true" : "false")}\n" +
                               $"BotToken={botToken ?? ""}\n" +
                               $"ChatId={chatId ?? ""}";
        File.WriteAllText(configFilePath, configContent);
    }
}
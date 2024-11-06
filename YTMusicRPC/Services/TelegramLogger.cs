using YTMusicRPC.Models;
using YTMusicRPC.utils;

namespace YTMusicRPC.Services;

public static class TelegramLogger
{
    private static Logger logger = Logger.Instance;
    private static readonly Config _config = ConfigManager.Config;

    static TelegramLogger(){
        
    }
    
    public static async Task SendLogAsync(string message) {
        if (!_config.AnalyticsEnabled) {
            // analytic disabled
            return;
        }
        
        if (string.IsNullOrEmpty(_config.BotToken) || string.IsNullOrEmpty(_config.ChatId)) {
            logger.LogWarning("Telegram bot token or chat ID is missing. Please update the configuration.");
            ConsoleHandler.RestoreFromTray();
            SaveTrackHistory.RequestAnalytics();
            return;
        }

        using (HttpClient client = new HttpClient()) {
            var url = $"https://api.telegram.org/bot{_config.BotToken}/sendMessage?chat_id={_config.ChatId}&text={Uri.EscapeDataString(message)}&parse_mode=Markdown";
            HttpResponseMessage response = await client.GetAsync(url);
        }
    }
}
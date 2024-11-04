using people2json.utils;

public static class TelegramLogger
{
    static Logger logger = new Logger();
    private static readonly string botToken = ConfigManager.GetBotToken();
    private static readonly string chatId = ConfigManager.GetChatId();
    private static readonly bool analyticsEnabled = ConfigManager.IsAnalyticsEnabled();

    public static async Task SendLogAsync(string message)
    {
        if (!analyticsEnabled)
        {
            // analytic disabled
            return;
        }
        
        if (string.IsNullOrEmpty(botToken) || string.IsNullOrEmpty(chatId))
        {
            logger.LogWarning("Telegram bot token or chat ID is missing. Please check the configuration.");
            return;
        }

        using (HttpClient client = new HttpClient())
        {
            var url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}&parse_mode=Markdown";
            HttpResponseMessage response = await client.GetAsync(url);
        }
    }
}
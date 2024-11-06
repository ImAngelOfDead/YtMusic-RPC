using YTMusicRPC.Models;

namespace YTMusicRPC.utils;

public static class SaveTrackHistory
{
    private static Logger _logger;
    
    static SaveTrackHistory()
    {
        _logger = Logger.Instance; 
    }
    public static Config RequestAnalytics() {
        _logger.LogInfo("Would you like to save track history? (y/n): ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("> ");

        string input = Console.ReadLine()?.Trim().ToLower();
        Console.ResetColor();
        bool isAnalyticsEnabled = input == "y";

        if (isAnalyticsEnabled) {
            _logger.LogInfo("Please enter your Telegram Bot Token: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
            string botToken = Console.ReadLine();

            _logger.LogInfo("Please enter your Telegram Chat ID: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
            string chatId = Console.ReadLine();

            return new Config() { AnalyticsEnabled = isAnalyticsEnabled, BotToken = botToken, ChatId = chatId };
        }
        else {
            _logger.LogInfo("Okay!");
            return new Config() { AnalyticsEnabled = isAnalyticsEnabled };
            
        }
    }
}
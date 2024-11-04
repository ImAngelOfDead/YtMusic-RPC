using people2json.utils;
public static class ConfigManager
{
    private static readonly string configFilePath = "config.txt";
    static Logger logger = new Logger();

    public static bool IsAnalyticsEnabled()
    {
        if (!File.Exists(configFilePath))
        {
            return RequestAnalyticsPermission();
        }

        var configContent = File.ReadAllText(configFilePath);
        return configContent.Contains("AnalyticsEnabled=true");
    }

    public static string GetBotToken()
    {
        return GetConfigValue("BotToken");
    }

    public static string GetChatId()
    {
        return GetConfigValue("ChatId");
    }

    private static string GetConfigValue(string key)
    {
        if (!File.Exists(configFilePath)) return null;

        var configLines = File.ReadAllLines(configFilePath);
        foreach (var line in configLines)
        {
            if (line.StartsWith($"{key}="))
            {
                return line.Substring($"{key}=".Length).Trim();
            }
        }

        return null;
    }

    private static bool RequestAnalyticsPermission()
    {
        logger.LogInfo("Would you like to save track history? (y/n): ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("> ");

        string input = Console.ReadLine()?.Trim().ToLower();
        Console.ResetColor();
        bool isAnalyticsEnabled = input == "y";

        if (isAnalyticsEnabled)
        {
            logger.LogInfo("Please enter your Telegram Bot Token: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
            string botToken = Console.ReadLine();

            logger.LogInfo("Please enter your Telegram Chat ID: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("> ");
            string chatId = Console.ReadLine();

            SaveConfig(isAnalyticsEnabled, botToken, chatId);
        }
        else
        {
            SaveConfig(isAnalyticsEnabled);
        }

        return isAnalyticsEnabled;
    }

    private static void SaveConfig(bool isAnalyticsEnabled, string botToken = null, string chatId = null) 
    {
        string configContent = $"AnalyticsEnabled={(isAnalyticsEnabled ? "true" : "false")}\n" +
                               $"BotToken={botToken ?? ""}\n" +
                               $"ChatId={chatId ?? ""}";
        File.WriteAllText(configFilePath, configContent);
    }
}

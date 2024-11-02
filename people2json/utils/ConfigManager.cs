using System;
using System.IO;
using System.Threading.Tasks;
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

    private static bool RequestAnalyticsPermission()
    {
        logger.LogInfo("Do you allow anonymous data collection and reporting? (y/n): ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("> ");
        
        string input = Console.ReadLine()?.Trim().ToLower();
        Console.ResetColor();
        bool isAnalyticsEnabled = input == "y";

        SaveConfig(isAnalyticsEnabled);
        return isAnalyticsEnabled;
    }

    private static void SaveConfig(bool isAnalyticsEnabled)
    {
        string configContent = $"AnalyticsEnabled={(isAnalyticsEnabled ? "true" : "false")}";
        File.WriteAllText(configFilePath, configContent);
    }
}
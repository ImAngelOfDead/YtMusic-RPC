using Microsoft.Win32;
using YTMusicRPC.Models;

namespace YTMusicRPC.utils;

public static class ConfigManagerRegedit
{
    private const string RegistryPath = @"Software\YTMusicRPC\Config";
    private const string ANALYTICS_ENABLED_KEY = "AnalyticsEnabled"; 
    private const string BOT_TOKEN_KEY = "BOT_TOKEN"; 
    private const string CHAT_ID_KEY = "CHAT_ID"; 
    
    private static Logger _logger = Logger.Instance;
    public static Config Config{ get; private set; } = new Config();
    
    static ConfigManagerRegedit(){
        bool? isEnabled = IsAnalyticsEnabled();
        if (isEnabled.HasValue && isEnabled.Value) {
            Config = GetConfig();
            return;
        }
        Config = SaveTrackHistory.RequestAnalytics();
        SaveConfig(Config);
    }
    public static void Initialize(){
        Console.Clear();
    }

    private static Config GetConfig(){
        var config = new Config() { AnalyticsEnabled = IsAnalyticsEnabled().Value, BotToken = GetBotToken(), ChatId = GetChatId() };
        return config; 
    }
    
    private static void SaveConfig(Config config){
        SaveBoolSetting(ANALYTICS_ENABLED_KEY, config.AnalyticsEnabled);
        SaveStringSetting(BOT_TOKEN_KEY, config.BotToken);
        SaveStringSetting(CHAT_ID_KEY, config.ChatId);
    }
    private static bool? IsAnalyticsEnabled()
    {
        return ReadBoolSetting(ANALYTICS_ENABLED_KEY);
    }
    private static string GetBotToken()
    {
        return ReadStringSetting(BOT_TOKEN_KEY);
    }
    
    private static string GetChatId()
    {
        return ReadStringSetting(CHAT_ID_KEY);
    }

    private static void SaveStringSetting(string key, string value)
    {
        using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
        {
            registryKey.SetValue(key, value, RegistryValueKind.String);
        }
    }

    private static void SaveBoolSetting(string key, bool value)
    {
        using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
        {
            registryKey.SetValue(key, value, RegistryValueKind.DWord);
        }
    }

    private static string ReadStringSetting(string key)
    {
        using (RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
        {
            return registryKey?.GetValue(key, "") as string ?? "";
        }
    }

    private static bool? ReadBoolSetting(string key)
    {
        using (RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
        {
            var value = registryKey?.GetValue(key, null);
            
            switch (value){
                case null:
                case int intValue when intValue != 1 && intValue != 0:
                    return null;
                case int intValue:
                    return Convert.ToBoolean(intValue); 
                default:
                    return null;
            }
        }
    }
}
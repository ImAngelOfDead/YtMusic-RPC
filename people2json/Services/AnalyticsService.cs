using System.Runtime.InteropServices;
using people2json.utils;
public static class AnalyticsService
{
    public static async Task CollectAndSendAnalyticsAsync(string appVersion)
    {
        string analyticsData = CollectAnalyticsData(appVersion);
        await TelegramLogger.SendLogAsync(analyticsData);
    }

    private static string CollectAnalyticsData(string appVersion)
    {
        string osInfo = RuntimeInformation.OSDescription;
        string architecture = RuntimeInformation.OSArchitecture.ToString();
        string currentTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " UTC";
        #if DEBUG
            string channel = "Debug";
        #else
            string channel = "Release";
        #endif
        
        return $"[Analytics Report]\n" +
               $"App Version: {appVersion}\n" +
               $"OS: {osInfo}\n" +
               $"Architecture: {architecture}\n" +
               $"Time: {currentTime}\n" +
               $"Channel: {channel}\n";
                
    }
}
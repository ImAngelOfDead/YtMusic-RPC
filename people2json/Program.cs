using System;
using people2json.utils;
using people2json.Services;
using people2json.Models;
using System.Threading.Tasks;

namespace people2json
{
    class Program
    {
        private static string LastVersion = "N\\A";
        static string version = "1.0.1";
        static string author = "m3th4d0n";
        private static string githubUrl = "https://github.com/M3th4d0n/YtMusic-RPC";
        static Logger logger = new Logger();

        static async Task Main()
        {
            LastVersion = await GithubService.GetLatestVersionAsync();
            
            logger.LogInfo("Program initialized");
            bool isAnalyticsEnabled = ConfigManager.IsAnalyticsEnabled();
        
            if (isAnalyticsEnabled)
            {
                await AnalyticsService.CollectAndSendAnalyticsAsync(version);
            }

            logger.LogInfo($"Author: {author}");
            logger.LogInfo("Current version: " + version);

            if (IsNewerVersion(LastVersion, version))
            {
                logger.LogInfo("Latest version: " + LastVersion);
                logger.LogWarning("A newer version is available. Please consider updating.");
            }

            logger.LogInfo($"Github URL: {githubUrl}");

            var discordService = new DiscordService("1194717480627740753");
            discordService.Initialize();

            var webSocketService = new WebSocketService("/trackInfo", discordService);
            webSocketService.Start();
            
            Console.ReadKey();

            webSocketService.Stop();
            discordService.Dispose();
        }
        
        static bool IsNewerVersion(string lastVersion, string currentVersion)
        {
            var last = new Version(lastVersion);
            var current = new Version(currentVersion);
            return last > current;
        }
    }
}

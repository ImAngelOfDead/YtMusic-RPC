using System;
using people2json.utils;
using people2json.Services;
using people2json.Models;
namespace people2json
{
    class Program
    {
        static string version = "1.0.0";
        static string author = "m3th4d0n";
        static Logger logger = new Logger();

        static void Main()
        {
            logger.LogInfo("Program initialized");
            logger.LogInfo($"Version: {version}, Author: {author}");

            var discordService = new DiscordService("1194717480627740753");
            discordService.Initialize();

            var webSocketService = new WebSocketService("/trackInfo", discordService);
            webSocketService.Start();
            
            Console.ReadKey();

            webSocketService.Stop();
            discordService.Dispose();
        }
    }
}
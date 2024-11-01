using Newtonsoft.Json;
using people2json.utils;
using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using DiscordRPC;
using DiscordRPC.Logging;
using LogLevel = DiscordRPC.Logging.LogLevel;

class Program
{
    static string version = "1.0.0";
    static string author = "m3th4d0n";
    static people2json.utils.Logger logger = new people2json.utils.Logger();
    static DiscordRpcClient client;

    static void Init()
    {
        logger.LogInfo("Program initialized");
        logger.LogInfo("Version: " + version);
        logger.LogInfo("Author: " + author);

        IsDebug debug = new IsDebug();
        debug.DebugCheck();

        client = new DiscordRpcClient("1194717480627740753");
        client.Initialize();
    }

    public class TrackInfo
    {
        public string Track { get; set; }
        public string Artist { get; set; }
        public string Cover { get; set; }
    }

    public class TrackInfoBehavior : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            TrackInfo trackInfo = JsonConvert.DeserializeObject<TrackInfo>(e.Data);
            if (trackInfo != null)
            {
                string output = $"Track Info:\n" +
                                $"  Title: {trackInfo.Track}\n" +
                                $"  Artist: {trackInfo.Artist}\n" +
                                $"  Cover: {trackInfo.Cover}\n";

                SendTrackInfoToRpc(trackInfo);
            }
            else
            {
                logger.LogError("Failed to deserialize data: " + e.Data);
            }
        }

        private void SendTrackInfoToRpc(TrackInfo trackInfo)
        {
            
            client.SetPresence(new RichPresence()
            {
                Details = trackInfo.Track,
                State = trackInfo.Artist,
                Buttons = new Button[]
                {
                    new Button { Label = "View on GitHub", Url = "https://github.com/m3th4d0n/YtMusic-RPC" }
                },
                Assets = new Assets()
                {
                    LargeImageKey = "track_cover",
                    LargeImageText = trackInfo.Track
                }
                
            });
        }

    }

    static void StartWebSocketServer()
    {
        var webSocketServer = new WebSocketServer("ws://localhost:5000");
        webSocketServer.AddWebSocketService<TrackInfoBehavior>("/trackInfo");
        webSocketServer.Start();
        logger.LogInfo("WebSocket server started on ws://localhost:5000/trackInfo");
    }

    static void Main()
    {
        Init();
        
        StartWebSocketServer();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        client.Dispose();
    }
}

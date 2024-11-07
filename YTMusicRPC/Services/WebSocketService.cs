using WebSocketSharp.Server;
using Logger = YTMusicRPC.utils.Logger;

namespace YTMusicRPC.Services;

public class WebSocketService
{
    public Logger logger = Logger.Instance;
    private readonly WebSocketServer _server;
    private readonly DiscordService _discordService;
    private readonly HistoryService _historyService = HistoryService.Instance;

    public WebSocketService(string path, DiscordService discordService){
        _server = new WebSocketServer("ws://localhost:5000");
        _discordService = discordService;
        _server.AddWebSocketService(path, () => new TrackInfoProcessor(_discordService));
    }

    public void Start(){
        _server.Start();
        logger.LogSocket("WebSocket server started on ws://localhost:5000/trackInfo");
    }

    public void Stop(){
        if (_server.IsListening){
            _server.Stop();
            logger.LogInfo("WebSocket server stopped.");
        }
    }
}
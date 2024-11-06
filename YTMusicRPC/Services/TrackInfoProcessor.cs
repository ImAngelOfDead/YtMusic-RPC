using WebSocketSharp;
using WebSocketSharp.Server;
using YTMusicRPC.Models;
using Newtonsoft.Json;
using Logger = YTMusicRPC.utils.Logger;

namespace YTMusicRPC.Services;

public class TrackInfoProcessor : WebSocketBehavior
{
    private Logger _logger = Logger.Instance;
    private readonly DiscordService _discordService;

    public TrackInfoProcessor(DiscordService discordService){
        _discordService = discordService;
    }

    protected override void OnMessage(MessageEventArgs e){
        var trackInfo = JsonConvert.DeserializeObject<TrackInfo>(e.Data);

        if (trackInfo != null){
            bool isTrackChanged = _discordService.LastVideoId != trackInfo.VideoId;

            _discordService.UpdatePresence(trackInfo.Track, trackInfo.Artist, trackInfo.Cover, trackInfo.CurrentTime,
                trackInfo.VideoId, trackInfo.IsPlaying);

            if (isTrackChanged){
                trackInfo.Artist = trackInfo.Artist.Replace("\n", "").Replace("\r", "");
                string telegramMessage = $"🎵 Now Playing:\nArtist: {trackInfo.Artist}\n" +
                                         $"Time: {DateTime.Now.ToString("MM/dd/yyyy hh:mm")}\n" +
                                         $"[Listen on YouTube Music](https://music.youtube.com/watch?v={trackInfo.VideoId})";

                Task.Run(() => TelegramLogger.SendLogAsync(telegramMessage));
            }
        }
        else{
            _logger.LogError("Failed to deserialize data: " + e.Data);
        }
    }
}
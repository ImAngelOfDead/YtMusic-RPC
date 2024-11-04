using WebSocketSharp;
using WebSocketSharp.Server;
using people2json.Models;
using Newtonsoft.Json;
using people2json.utils;
using Logger = people2json.utils.Logger;

namespace people2json.Services
{
    public class TrackInfoProcessor : WebSocketBehavior
    {
        public Logger logger = new Logger();
        private readonly DiscordService _discordService;

        public TrackInfoProcessor(DiscordService discordService)
        {
            _discordService = discordService;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var trackInfo = JsonConvert.DeserializeObject<TrackInfo>(e.Data);

            if (trackInfo != null)
            { //string track, string artist, string cover, int currentTime, int totalDuration
                _discordService.UpdatePresence(trackInfo.Track, trackInfo.Artist, trackInfo.Cover, trackInfo.CurrentTime);
                
            }
            
            else
            {
                logger.LogError("Failed to deserialize data: " + e.Data);
            }
        }
    }
}
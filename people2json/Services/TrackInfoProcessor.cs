using WebSocketSharp;
using WebSocketSharp.Server;
using people2json.Models;
using Newtonsoft.Json;
using people2json.utils;
namespace people2json.Services
{
    public class TrackInfoProcessor : WebSocketBehavior
    {
        
        private readonly DiscordService _discordService;

        public TrackInfoProcessor(DiscordService discordService)
        {
            _discordService = discordService;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var trackInfo = JsonConvert.DeserializeObject<TrackInfo>(e.Data);
            if (trackInfo != null)
            {
                _discordService.UpdatePresence(trackInfo.Track, trackInfo.Artist, trackInfo.Cover);
            }
            else
            {
                Console.WriteLine("Failed to deserialize data: " + e.Data);
            }
        }
    }
}
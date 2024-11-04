using WebSocketSharp;
using WebSocketSharp.Server;
using people2json.Models;
using Newtonsoft.Json;
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
            {
                // Проверка на смену трека или исполнителя
                bool isTrackChanged = _discordService.LastTrack != trackInfo.Track || _discordService.LastArtist != trackInfo.Artist;

                if (isTrackChanged)
                {
                    // Обновляем Presence в Discord и сохраняем текущий трек и исполнителя
                    _discordService.UpdatePresence(trackInfo.Track, trackInfo.Artist, trackInfo.Cover, trackInfo.CurrentTime, trackInfo.VideoId, trackInfo.IsPlaying);

                    // Формируем сообщение для отправки в Telegram
                    string telegramMessage = $"🎵 Now Playing:\nTrack: {trackInfo.Track}\nArtist: {trackInfo.Artist}\n" +
                                             $"[Listen on YouTube Music](https://music.youtube.com/watch?v={trackInfo.VideoId})";

                    // Отправка сообщения в Telegram
                    Task.Run(() => TelegramLogger.SendLogAsync(telegramMessage));
                }
            }
            else
            {
                logger.LogError("Failed to deserialize data: " + e.Data);
            }
        }
    }
}
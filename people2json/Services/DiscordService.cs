using DiscordRPC;
using Button = DiscordRPC.Button;

namespace people2json.Services
{
    public class DiscordService
    {
        private DiscordRpcClient _client;
        private string _lastTrack;
        private string _lastArtist;
        private DateTime _startTime;

        public DiscordService(string clientId)
        {
            _client = new DiscordRpcClient(clientId);
        }

        public void Initialize()
        {
            _client.Initialize();
        }

        public void UpdatePresence(string track, string artist, string cover, int currentTime)
        {
            // i hope...
            var trackLimited = track.Length > 64 ? track.Substring(0, 64) : track;
            var artistLimited = artist.Length > 64 ? artist.Substring(0, 64) : artist;

            _startTime = DateTime.UtcNow.AddSeconds(-currentTime);
            
            if (_lastTrack != trackLimited || _lastArtist != artistLimited)
            {
                _lastTrack = trackLimited;
                _lastArtist = artistLimited;
            }

            try
            {
                var presence = new RichPresence
                {
                    Details = trackLimited,
                    State = artistLimited,
                    Assets = new Assets
                    {
                        LargeImageKey = cover,
                        LargeImageText = trackLimited
                    },
                    Timestamps = new Timestamps
                    {
                        Start = _startTime
                    },
                    Buttons = new[]
                    {
                        new Button { Label = "github link", Url = "https://github.com/M3th4d0n/YtMusic-RPC" }
                    }
                };
                _client.SetPresence(presence);
            }
            catch (DiscordRPC.Exceptions.StringOutOfRangeException ex)
            {
                Console.WriteLine($"[ERROR] Discord RPC Exception: {ex.Message}");
                Console.WriteLine($"[ERROR] Track: '{trackLimited}', Artist: '{artistLimited}'");
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

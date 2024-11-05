using System.Diagnostics;
using DiscordRPC;
using Button = DiscordRPC.Button;

namespace people2json.Services {
    public class DiscordService {
        private DiscordRpcClient _client;
        
        private string _lastTrack;
        private string _lastArtist;
        private string _lastId;
        private DateTime _startTime;
        private Button _linkButton = new Button();
        private Button downloadButton = new Button();
        
        public string LastArtist{
            get{ return _lastArtist; }
            set{
                if(_lastArtist == value) return;
                _lastArtist = value;
            }
        }
        public string LastTrack{
            get{ return _lastTrack; }
            set{
                if(_lastTrack == value) return;
                _lastTrack = value;
            }
        }
        public string LastVideoId{
            get => _lastId;
            set{
                if(_lastId == value) return;
                _lastId = value;
                UpdateButton(_lastId);
            }
        }
        
        public DiscordService(string clientId) {
            _client = new DiscordRpcClient(clientId);
        }

        public void Initialize() {
            _client.Initialize();
            downloadButton = new Button
                { Label = "Download", Url = "https://github.com/M3th4d0n/YtMusic-RPC" };
        }

        public void UpdatePresence(string track, string artist, string cover, int currentTime, string videoId, bool isPlaying = true) {
            var trackLimited = track.Length > 64 ? track.Substring(0, 64) : track;
            var artistLimited = artist.Length > 64 ? artist.Substring(0, 64) : artist;

            _startTime = DateTime.UtcNow.AddSeconds(-currentTime);

            LastVideoId = videoId;
            LastTrack = trackLimited;
            LastArtist = artistLimited;

            try {
                if (!isPlaying){
                    _client.SetPresence(null);
                    return;
                }
                var presence = new RichPresence {
                    Details = trackLimited,
                    State = artistLimited,
                    Assets = new Assets {
                        LargeImageKey = cover,
                        LargeImageText = trackLimited
                    },
                    Timestamps = new Timestamps {
                        Start = _startTime,
                    },
                    Buttons = new[] {
                        _linkButton,
                        downloadButton
                    }
                };
                _client.SetPresence(presence);
            }
            catch (DiscordRPC.Exceptions.StringOutOfRangeException ex) {
                Console.WriteLine($"[ERROR] Discord RPC Exception: {ex.Message}");
                Console.WriteLine($"[ERROR] Track: '{trackLimited}', Artist: '{artistLimited}'");
            }
        }

        private void UpdateButton(string id) {
            _linkButton = new Button
                { Label = "Listen", Url = $"https://music.youtube.com/watch?v={id}" };
        }

        public void Dispose() {
            _client.Dispose();
        }
    }
}

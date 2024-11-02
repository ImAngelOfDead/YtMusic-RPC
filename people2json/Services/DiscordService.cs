using DiscordRPC;
using System;

namespace people2json.Services
{
    public class DiscordService : IDisposable
    {
        private readonly DiscordRpcClient _client;
        private System.Timers.Timer _rpcUpdateTimer;

        public DiscordService(string clientId)
        {
            _client = new DiscordRpcClient(clientId);
        }

        public void Initialize()
        {
            _client.Initialize();
            _rpcUpdateTimer = new System.Timers.Timer(15000);
            _rpcUpdateTimer.Elapsed += (sender, e) => _client.Invoke();
            _rpcUpdateTimer.Start();
        }

        public void UpdatePresence(string track, string artist, string cover)
        {
            _client.SetPresence(new RichPresence
            {
                Details = track,
                State = artist,
                Buttons = new[] { new Button { Label = "View on GitHub", Url = "https://github.com/m3th4d0n/YtMusic-RPC" } },
                Assets = new Assets
                {
                    LargeImageKey = "track_cover",
                    LargeImageText = track
                }
            });
        }

        public void Dispose()
        {
            _rpcUpdateTimer?.Stop();
            _client.Dispose();
        }
    }
}
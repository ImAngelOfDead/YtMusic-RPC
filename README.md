# YouTube Music Discord RPC

This project displays the current track from YouTube Music as a Discord Rich Presence, allowing you to share the song title, artist, and album art with friends on Discord in real-time. It uses a WebSocket connection to receive track data and integrates it into Discord via Discord RPC.

## Features

- **Real-time Discord Rich Presence**: Updates your Discord status with the currently playing track from YouTube Music.
- **Lightweight and Efficient**: Uses a WebSocket connection for seamless data transfer.

## Installation

To set up this project, you need to install the following component:

### YouTube Music Extension

First, install the **YouTube Music WebSocket Tracker** extension from [Greasy Fork](https://greasyfork.org/ru/scripts/515130-youtube-music-websocket-tracker).  
This extension sends track information from YouTube Music to a local WebSocket server, which then relays it to Discord.

- **Direct Download**: [YouTube Music WebSocket Tracker](https://greasyfork.org/ru/scripts/515130-youtube-music-websocket-tracker)

## Usage

1. Start playing music on [YouTube Music](https://music.youtube.com/).
2. Ensure the YouTube Music WebSocket Tracker extension is active.
3. Run the Discord RPC application you downloaded.
4. Your Discord status should now update to display the current track information from YouTube Music.

## Updating

Check for new releases of this application [here](https://github.com/M3th4d0n/YtMusic-RPC/releases/latest).

## Troubleshooting

- **Track Information Not Updating**: Ensure both the WebSocket tracker extension and the Discord RPC application are running.
- **Connection Issues**: Confirm that the WebSocket server is accessible on `ws://localhost:5000/trackInfo`. The extension and application require this connection to function.

## Contributing

Feel free to submit pull requests or report issues to improve functionality or suggest new features.

## License

This project is licensed under the MIT License.

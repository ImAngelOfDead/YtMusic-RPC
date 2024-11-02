# YouTube Music Discord RPC

This project displays the current track from YouTube Music as a Discord Rich Presence using a WebSocket connection and Discord RPC.

## Installation

To set up this project, you need to install two components:

1. **YouTube Music Extension**

   First, install the **YouTube Music WebSocket Tracker** extension from [Greasy Fork](https://greasyfork.org/ru/scripts/515130-youtube-music-websocket-tracker).  
   This extension will send track information from YouTube Music to a local WebSocket server.

   - **Direct Download**: [YouTube Music WebSocket Tracker Script](https://update.greasyfork.org/scripts/515130/YouTube%20Music%20WebSocket%20Tracker.user.js)

2. **Run the Discord RPC Application**

   - Clone this repository and navigate into the project directory.
   - Ensure you have [.NET](https://dotnet.microsoft.com/) installed, as this application uses .NET.
   - Run the application to start the WebSocket server and connect to Discord.

   ```bash
   git clone https://github.com/yourusername/your-repo.git
   cd your-repo
   dotnet run

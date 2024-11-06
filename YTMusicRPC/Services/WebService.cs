using System.Net;
using System.Text;
using Newtonsoft.Json;
using YTMusicRPC.utils;

namespace YTMusicRPC.Services
{
    public class WebServer
    {
        private static readonly Logger logger = Logger.Instance;
        private readonly HttpListener _listener;
        public const string Domain = "http://localhost:1337/";
        
        public WebServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(Domain);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            logger.LogInfo($"Web server started on {Domain}");

            while (true)
            {
                var context = await _listener.GetContextAsync();
                _ = Task.Run(() => ProcessRequestAsync(context));
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "POST")
            {
                await HandlePostRequest(context);
            }
            else
            {
                await HandleGetRequest(context);
            }
        }

        private async Task HandleGetRequest(HttpListenerContext context)
        {
            string htmlContent = GetHtmlContent();
            byte[] buffer = Encoding.UTF8.GetBytes(htmlContent);

            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = "text/html";
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        private async Task HandlePostRequest(HttpListenerContext context)
        {
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                string body = await reader.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(body);

                
                ConfigManager.Config.ChatId = data.ChatId;
                ConfigManager.Config.BotToken = data.Token;
                ConfigManager.SaveConfig(ConfigManager.Config);
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            byte[] responseBuffer = Encoding.UTF8.GetBytes("Configuration updated successfully!");
            await context.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
            context.Response.OutputStream.Close();
        }

        private string GetHtmlContent()
        {
                return $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <title>YTMusicRPC Configuration</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f9;
                    color: #333;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    margin: 0;
                }}
                .container {{
                    width: 300px;
                    padding: 20px;
                    background-color: #fff;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    border-radius: 8px;
                }}
                h1 {{
                    font-size: 1.5em;
                    text-align: center;
                    color: #555;
                }}
                label {{
                    font-weight: bold;
                    margin-top: 10px;
                    display: block;
                    color: #555;
                }}
                input[type='text'] {{
                    width: 100%;
                    padding: 8px;
                    margin-top: 5px;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    box-sizing: border-box;
                }}
                button {{
                    width: 100%;
                    padding: 10px;
                    background-color: #4CAF50;
                    color: white;
                    border: none;
                    border-radius: 4px;
                    cursor: pointer;
                    font-size: 1em;
                }}
                button:hover {{
                    background-color: #45a049;
                }}
                #responseMessage {{
                    margin-top: 15px;
                    font-size: 0.9em;
                    color: #4CAF50;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <h1>YTMusicRPC Config</h1>
                <form id=""configForm"">
                    <label for=""chatId"">Chat ID:</label>
                    <input type=""text"" id=""chatId"" name=""ChatId"" value=""{ConfigManager.Config.ChatId}""><br>

                    <label for=""token"">Token:</label>
                    <input type=""text"" id=""token"" name=""Token"" value=""{ConfigManager.Config.BotToken}""><br>

                    <button type=""button"" onclick=""updateConfig()"">Save</button>
                </form>
                <p id=""responseMessage""></p>
            </div>

            <script>
                async function updateConfig() {{
                    const chatId = document.getElementById('chatId').value;
                    const token = document.getElementById('token').value;
                    const response = await fetch('', {{
                        method: 'POST',
                        headers: {{
                            'Content-Type': 'application/json'
                        }},
                        body: JSON.stringify({{ ChatId: chatId, Token: token }})
                    }});
                    const message = await response.text();
                    document.getElementById('responseMessage').innerText = message;
                }}
            </script>
        </body>
        </html>";
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            Console.WriteLine("Web server stopped.");
        }
    }
}

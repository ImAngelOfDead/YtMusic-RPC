using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using YTMusicRPC.utils;

namespace YTMusicRPC.Services;

public class WebServer
{
    private static readonly Logger logger = Logger.Instance;
    private readonly HttpListener _listener;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    public const string Domain = "http://localhost:1337/";

    public WebServer(){
        _listener = new HttpListener();
        _listener.Prefixes.Add(Domain);
    }

    public async Task StartAsync()
    {
        _listener.Start();
        logger.LogInfo($"Web server started on {Domain}");

        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var context = await _listener.GetContextAsync();
                
                _ = ProcessRequestAsync(context);
            }
        }
        catch (HttpListenerException ex) when (_cts.Token.IsCancellationRequested)
        {
            logger.LogInfo("Web server stopped gracefully.");
        }
        catch (Exception ex)
        {
            logger.LogError("Unexpected error: " + ex.Message);
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context){
        if (context.Request.HttpMethod == "POST"){
            await HandlePostRequest(context);
        }
        else{
            await HandleGetRequest(context);
        }
    }

    private async Task HandleGetRequest(HttpListenerContext context){
        string htmlContent = GetHtmlContent();
        byte[] buffer = Encoding.UTF8.GetBytes(htmlContent);

        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "text/html";
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }

    private async Task HandlePostRequest(HttpListenerContext context){
        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding)){
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

    private string GetHtmlContent(){
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "YTMusicRPC.Pages.Html.index.html";
        
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            var htmlTemplate = reader.ReadToEnd();

            var filledHtml = htmlTemplate
                .Replace("{ChatId}", ConfigManager.Config.ChatId)
                .Replace("{BotToken}", ConfigManager.Config.BotToken);

            return filledHtml;
        }
    }

    public void Stop(){
        _listener.Stop();
        _listener.Close();
        Console.WriteLine("Web server stopped.");
    }
}

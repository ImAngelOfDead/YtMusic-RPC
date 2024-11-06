using System.Diagnostics;
using System.Reflection;
using YTMusicRPC.Services;
using YTMusicRPC.utils;
using Spectre.Console;
using Panel = Spectre.Console.Panel;

// git reset --hard origin/master

namespace YTMusicRPC;

class Program
{
    
    private static readonly string UpdaterPath = Path.Combine(Directory.GetCurrentDirectory(), "updater.exe");
    private static string LastVersion = "N\\A";
    private static readonly string version = "1.1.0";
    private static readonly string authors = "m3th4d0n & Anfi1";
    private static readonly string githubUrl = "https://github.com/M3th4d0n/YtMusic-RPC";
    private static readonly Logger logger = Logger.Instance;
    private static NotifyIcon trayIcon;
    private static DiscordService discordService;
    private static WebSocketService webSocketService;

    
    [STAThread]
    static async Task Main(string[] args){
        
        
        Console.Title = "by m3th4d0n & Anfi1";
        
        ConfigManager.Initialize();
        
        if (await CheckForUpdateAsync()){
            await DownloadUpdaterAsync();
            StartUpdater(LastVersion);
        }

        ShowApplicationInfo();
        await InitializeServices();
        InitializeTrayIcon();
        
        logger.LogInfo("After a few seconds, program will disappear into tray");
        Thread.Sleep(3000);
        ConsoleHandler.MinimizeToTray();
        
        Application.Run();
    }

    private static async Task<bool> CheckForUpdateAsync(){
        LastVersion = await GithubService.GetLatestVersionAsync();
        if (CompareVersions(LastVersion, version) > 0){
            return true; 
        }
        return false;
    }

    public static int CompareVersions(string version1, string version2)
    {
        var v1Parts = version1.Split('.');
        var v2Parts = version2.Split('.');

        int maxLength = Math.Max(v1Parts.Length, v2Parts.Length);

        for (int i = 0; i < maxLength; i++)
        {
            int v1Value = i < v1Parts.Length ? int.Parse(v1Parts[i]) : 0;
            int v2Value = i < v2Parts.Length ? int.Parse(v2Parts[i]) : 0;

            if (v1Value < v2Value)
                return -1;  
            if (v1Value > v2Value)
                return 1;   
        }

        return 0;
    }
    private static async Task DownloadUpdaterAsync(){
        using var client = new HttpClient();
        var updaterUrl = "https://raw.githubusercontent.com/M3th4d0n/YtMusic-RPC/refs/heads/master/Updater.exe";
        var data = await client.GetByteArrayAsync(updaterUrl);
        await File.WriteAllBytesAsync(UpdaterPath, data);
    }

    private static void StartUpdater(string latestVersion){
        var updaterCommand = $"/C start \"\" \"{UpdaterPath}\" {latestVersion} && exit";
        var startInfo = new ProcessStartInfo{
            FileName = "cmd.exe",
            Arguments = updaterCommand,
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = true
        };
        Process.Start(startInfo);


        Environment.Exit(0);
    }


    private static void ShowApplicationInfo(){
        AnsiConsole.Write(
            new Panel(
                    $"[yellow]Author:[/] [cyan][link=https://github.com/M3th4d0n]m3th4d0n[/][/] [green]&[/] [cyan][link=https://github.com/Anfi1]Anfi1[/][/]\n" +
                    $"[yellow]Current Version:[/] [green]{version}[/]\n" +
                    $"[yellow]GitHub URL:[/] [link={githubUrl}]{githubUrl}[/]")
                .BorderColor(new Spectre.Console.Color(0, 255, 255))
                .Header("Application Information")
        );
        logger.LogInfo("Program initialized");

        if (!string.IsNullOrEmpty(LastVersion) && IsNewerVersion(LastVersion, version)){
            logger.LogWarning($"Latest version: {LastVersion}");
            logger.LogWarning("A newer version is available. Please consider updating");
        }
    }

    private static void InitializeTrayIcon(){
        Icon appIcon; // new System.Drawing.Icon("icon.ico")
        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("YTMusicRPC.Resources.icon.ico"))
        {
            appIcon = new Icon(stream);
        }
        trayIcon = new NotifyIcon{
            Icon = appIcon,
            Visible = true,
            Text = "YtMusic-RPC",
        };
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Hide to tray", null, (s, e) => ConsoleHandler.MinimizeToTray());
        contextMenu.Items.Add("Exit", null, (s, e) => OnExit(s, e));
        trayIcon.ContextMenuStrip = contextMenu;
        trayIcon.DoubleClick += (sender, e) => ConsoleHandler.RestoreFromTray();
    }

    private static async Task InitializeServices(){
        discordService = new DiscordService("1194717480627740753");
        discordService.Initialize();

        webSocketService = new WebSocketService("/trackInfo", discordService);
        webSocketService.Start();
        
        var webServer = new WebServer();
        await webServer.StartAsync();
        
    }

    private static bool IsNewerVersion(string lastVersion, string currentVersion){
        return new Version(lastVersion) > new Version(currentVersion);
    }

    private static void OnExit(object sender, EventArgs e){
        Cleanup();
    }

    private static void Cleanup(){
        webSocketService.Stop();
        discordService.Dispose();
        trayIcon.Dispose();
        Application.Exit();
    }
}
namespace people2json.utils;

public class Logger
{
    public void LogInfo(string message) {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[INFO]: {message}");
        Console.ResetColor();
    }

    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR]: {message}");
        Console.ResetColor();
    }

    public void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARN]: {message}");
        Console.ResetColor();
    }

    public void LogSocket(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[WebSocket]: {message}");
        Console.ResetColor();
    }
}
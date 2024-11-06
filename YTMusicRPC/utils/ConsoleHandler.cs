using System.Runtime.InteropServices;

namespace YTMusicRPC.utils;

public static class ConsoleHandler
{
    private static bool _isMinimizedToTray = false;
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    private static void SetConsoleWindowVisibility(bool visible){
        IntPtr hWnd = FindWindow(null, Console.Title);
        if (hWnd != IntPtr.Zero){
            ShowWindow(hWnd, visible ? 1 : 0); // 1 = SW_SHOWNORMAL, 0 = SW_HIDE
        }
    }
    public static void RestoreFromTray(){
        if (!_isMinimizedToTray){
            return;
        }
        SetConsoleWindowVisibility(true);
        _isMinimizedToTray = false;
    }

    public static void MinimizeToTray(){
        if (_isMinimizedToTray){
            return;
        }
        SetConsoleWindowVisibility(false);
        _isMinimizedToTray = true;
    }
}
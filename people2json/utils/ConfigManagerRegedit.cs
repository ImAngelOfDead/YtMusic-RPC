using Microsoft.Win32;

namespace YTMusicRPC.utils;

public static class ConfigManagerRegedit
{
    private const string RegistryPath = @"Software\YTMusicRPC\Config";

    public static void SaveStringSetting(string key, string value)
    {
        using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
        {
            registryKey.SetValue(key, value, RegistryValueKind.String);
        }
    }

    public static void SaveBoolSetting(string key, bool value)
    {
        using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath))
        {
            registryKey.SetValue(key, value, RegistryValueKind.DWord);
        }
    }

    public static string ReadStringSetting(string key)
    {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
        {
            return registryKey?.GetValue(key, "") as string ?? "";
        }
    }

    public static bool ReadBoolSetting(string key)
    {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
        {
            return Convert.ToBoolean((int)(registryKey?.GetValue(key, 0) ?? 0));
        }
    }
}
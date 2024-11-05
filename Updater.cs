using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Updater
{
    public class Program
    {
        private static readonly string RepoUrl = "https://github.com/M3th4d0n/YtMusic-RPC";
        private static string UpdateUrl;

        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("[ERROR] No version specified. Exiting updater.");
                return;
            }

            string version = args[0];
            UpdateUrl = $"{RepoUrl}/releases/download/{version}/release.zip";
            Console.WriteLine($"[INFO] Starting update process for version {version}...");

            var tempFile = Path.GetTempFileName();
            try
            {
                using (var client = new HttpClient())
                {
                    Console.WriteLine("[INFO] Downloading update...");
                    var data = await client.GetByteArrayAsync(UpdateUrl);
                    await File.WriteAllBytesAsync(tempFile, data);
                    Console.WriteLine("[INFO] Update downloaded. Preparing for extraction...");
                }

                string extractionPath = AppDomain.CurrentDomain.BaseDirectory;
                ExtractAndReplace(tempFile, extractionPath);

                Console.WriteLine("[INFO] Update applied successfully.");
        
                
                string mainAppPath = Path.Combine(extractionPath, "people2json.exe");
                Console.WriteLine("[INFO] Launching main application...");
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe")
                {
                    Arguments = $"/C start \"\" \"{mainAppPath}\" && exit",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Update failed: {ex.Message}");
                Console.WriteLine(UpdateUrl);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);

                SelfDelete();
            }
        }


        private static void ExtractAndReplace(string zipFilePath, string destinationDirectory)
        {
            using (var archive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    string entryPath = Path.Combine(destinationDirectory, entry.FullName);
                    
                    
                    string directoryPath = Path.GetDirectoryName(entryPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    
                    if (!entry.FullName.EndsWith("/"))
                    {
                        if (File.Exists(entryPath))
                        {
                            File.Delete(entryPath);
                        }

                        entry.ExtractToFile(entryPath, overwrite: true);
                    }
                }
            }
        }

        private static void SelfDelete()
        {
            var currentPath = Process.GetCurrentProcess().MainModule?.FileName;
            if (currentPath != null)
            {
                ProcessStartInfo info = new ProcessStartInfo("cmd.exe")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = $"/C timeout 2 && del \"{currentPath}\""
                };
                Process.Start(info);
                
            }
        }
    }
}

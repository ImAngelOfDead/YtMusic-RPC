using Newtonsoft.Json.Linq;

namespace YTMusicRPC.utils;

public static class GithubService
{
    private static readonly string apiUrl = "https://api.github.com/repos/M3th4d0n/YtMusic-RPC/releases/latest";

    public static async Task<string> GetLatestVersionAsync(){
        using (HttpClient client = new HttpClient()){
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            try{
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject release = JObject.Parse(responseBody);

                return release["tag_name"]?.ToString() ?? "Version not found";
            }
            catch (Exception ex){
                Console.WriteLine("Error fetching version from GitHub: " + ex.Message);
                return "Version fetch error";
            }
        }
    }
}
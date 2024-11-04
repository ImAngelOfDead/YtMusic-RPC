public static class TelegramLogger
{
    private static readonly string botToken = "7410071401:AAHJqtkRyC-fG_Cz7Km35_Sj8uv8M5_uQ00";
    private static readonly string chatId = "-1002225972188";

    public static async Task SendLogAsync(string message)
    {
        using (HttpClient client = new HttpClient())
        {
            var url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}&parse_mode=Markdown";

            HttpResponseMessage response = await client.GetAsync(url);
        }
    }
}
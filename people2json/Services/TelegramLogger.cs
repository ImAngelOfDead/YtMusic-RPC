public static class TelegramLogger
{
    private static readonly string botToken = "7410071401:AAGil1E-t13gnWL8ScWyuc1W6cXGerisBC8";
    private static readonly string chatId = "5972081387";

    public static async Task SendLogAsync(string message)
    {
        using (HttpClient client = new HttpClient())
        {
            var url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";
            
            HttpResponseMessage response = await client.GetAsync(url);

        }
    }
}
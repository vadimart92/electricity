namespace electricity;

class TelegramNotifier : INotifier
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TelegramNotifier(IHttpClientFactory httpClientFactory) {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Notify(Location location, LocationState state) {
        if (location.Id != LocationIds.Location1) {
            return;
        }
        using var telegramClient = _httpClientFactory.CreateClient("telegram");
        var botKey = Environment.GetEnvironmentVariable("TELEGRAM_BOT_KEY");
        var chatIt = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID");
        var text = state == LocationState.Offline ? "🔴 Світла немає" : "🟢 Світло є";
        var uri = $"https://api.telegram.org/bot{botKey}/sendMessage?chat_id={chatIt}&text={text}";
        await telegramClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
    }
}

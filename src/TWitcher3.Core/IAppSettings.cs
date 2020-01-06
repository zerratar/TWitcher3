using Newtonsoft.Json;

namespace TWitcher3.Providers
{
    public interface IAppSettings
    {
        string TwitchBotUsername { get; }
        string TwitchBotAuthToken { get; }
        string SpotifyApiSecret { get; }
        string SpotifyApiClientId { get; }
        string SpotifyApiCallbackUrl { get; }
    }

    public class AppSettingsProvider
    {
        public IAppSettings Get()
        {
            var text = System.IO.File.ReadAllText("settings.json");
            return JsonConvert.DeserializeObject<AppSettings>(text);
        }
    }

    public class AppSettings : IAppSettings
    {
        public AppSettings(string twitchBotUsername, string twitchBotAuthToken, string spotifyApiSecret, string spotifyApiClientId, string spotifyApiCallbackUrl)
        {
            TwitchBotUsername = twitchBotUsername;
            TwitchBotAuthToken = twitchBotAuthToken;
            SpotifyApiSecret = spotifyApiSecret;
            SpotifyApiClientId = spotifyApiClientId;
            SpotifyApiCallbackUrl = spotifyApiCallbackUrl;
        }

        public string TwitchBotUsername { get; }
        public string TwitchBotAuthToken { get; }
        public string SpotifyApiSecret { get; }
        public string SpotifyApiClientId { get; }
        public string SpotifyApiCallbackUrl { get; }
    }
}
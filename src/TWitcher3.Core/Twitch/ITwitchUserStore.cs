namespace TWitcher3.Core.Twitch
{
    public interface ITwitchUserStore
    {
        ITwitchUser Get(string username);
    }
}
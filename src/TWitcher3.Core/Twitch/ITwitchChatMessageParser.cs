using TWitcher3.Core.Chat;

namespace TWitcher3.Core.Twitch
{
    public interface ITwitchChatMessageParser
    {
        TwitchChatMessage Parse(TwitchChatSender twitchSender, string message,
            IChatMessageWordResolver wordResolver, IChatMessageEmoteResolver emotes);
    }
}
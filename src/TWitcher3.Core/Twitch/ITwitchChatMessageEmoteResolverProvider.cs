using TwitchLib.Client.Models;
using TWitcher3.Core.Chat;

namespace TWitcher3.Core.Twitch
{
    public interface ITwitchChatMessageEmoteResolverProvider
    {
        IChatMessageEmoteResolver Get(TwitchLib.Client.Models.EmoteSet emotes);
    }

    public class TwitchChatMessageEmoteResolverProvider : ITwitchChatMessageEmoteResolverProvider
    {
        private readonly IChatMessageEmoteRepository emoteRepository;

        public TwitchChatMessageEmoteResolverProvider(IChatMessageEmoteRepository emoteRepository)
        {
            this.emoteRepository = emoteRepository;
        }

        public IChatMessageEmoteResolver Get(EmoteSet emotes)
        {
            return new TwitchChatMessageEmoteResolver(emotes, emoteRepository);
        }
    }
}
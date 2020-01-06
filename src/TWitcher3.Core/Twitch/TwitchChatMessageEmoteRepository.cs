using System.Linq;
using TWitcher3.Core.Chat;
using TWitcher3.Core.Repositories;

namespace TWitcher3.Core.Twitch
{
    public class TwitchChatMessageEmoteRepository
        : FileBasedRepository<IChatEmote, TwitchChatEmote>, IChatMessageEmoteRepository
    {
        public TwitchChatMessageEmoteRepository()
            : base("E:\\stream\\emotes.json")
        {
        }

        public IChatEmote Find(string name)
        {
            var emotes = All();
            return emotes.FirstOrDefault(x => x.Name == name);
        }
    }
}
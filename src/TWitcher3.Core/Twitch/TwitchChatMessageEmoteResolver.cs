using System.Linq;
using System.Runtime.CompilerServices;
using TwitchLib.Client.Models;
using TWitcher3.Core.Chat;

namespace TWitcher3.Core.Twitch
{
    public class TwitchChatMessageEmoteResolver : IChatMessageEmoteResolver
    {
        private readonly EmoteSet emoteSet;
        private readonly IChatMessageEmoteRepository emoteRepository;

        public TwitchChatMessageEmoteResolver(
            TwitchLib.Client.Models.EmoteSet emoteSet,
            IChatMessageEmoteRepository emoteRepository)
        {
            this.emoteSet = emoteSet;
            this.emoteRepository = emoteRepository;
        }

        public IChatEmote Resolve(string token, bool highResIfPossible = false)
        {
            // the repository will always have overriding effect
            var chatEmote = emoteRepository.Find(token);
            if (chatEmote != null)
            {
                return chatEmote;
            }

            if (emoteSet == null || emoteSet.Emotes.Count <= 0)
            {
                return null;
            }

            var emote = emoteSet.Emotes.FirstOrDefault(x => x.Name == token);
            if (emote != null)
            {
                var imageUrl = highResIfPossible ? HighResTwitchEmote(emote.ImageUrl) : emote.ImageUrl;
                return new TwitchChatEmote(emote.Name, imageUrl);
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string HighResTwitchEmote(string input)
        {
            return input.Remove(input.LastIndexOf('/')) + "/3.0";
        }
    }
}
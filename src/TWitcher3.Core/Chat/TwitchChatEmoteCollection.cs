using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace TWitcher3.Core.Twitch
{
    public class TwitchChatEmoteCollection
    {
        public TwitchChatEmoteCollection(ChatMessage message, IEnumerable<string> emoteUrls)
        {
            Message = message;
            EmoteUrls = emoteUrls.ToList();
        }

        public ChatMessage Message { get; }

        public IReadOnlyList<string> EmoteUrls { get; }
    }
}
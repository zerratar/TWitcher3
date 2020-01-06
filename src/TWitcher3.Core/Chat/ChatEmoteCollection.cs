using System.Collections.Generic;
using System.Linq;

namespace TWitcher3.Core.Twitch
{
    public class ChatEmoteCollection
    {
        public ChatEmoteCollection(string sender, IEnumerable<string> emoteUrls)
        {
            Sender = sender;
            EmoteUrls = emoteUrls.ToList();
        }

        public string Sender { get; }
        public IReadOnlyList<string> EmoteUrls { get; }
    }
}
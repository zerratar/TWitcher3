using System.Collections.Generic;
using TWitcher3.Core.Chat;

namespace TWitcher3.Core.Twitch
{
    public class TwitchChatMessageParser : ITwitchChatMessageParser
    {
        public TwitchChatMessage Parse(
            TwitchChatSender sender,
            string message,
            IChatMessageWordResolver wordResolver,
            IChatMessageEmoteResolver emoteResolver)
        {
            var parts = new List<TwitchChatMessagePart>();
            var words = message.Split(' ');
            //var emotes = knownEmotes?.Emotes;
            var index = -1;
            foreach (var token in words)
            {
                ++index;
                var emote = emoteResolver.Resolve(token);
                if (emote != null)
                {
                    if (index > 0) parts.Add(new TwitchChatMessagePart("text", " "));
                    parts.Add(new TwitchChatMessagePart("emote", emote.ImageUrl));
                    continue;
                }

                var word = wordResolver.Resolve(token);
                if (index > 0) parts.Add(new TwitchChatMessagePart("text", " "));
                parts.Add(new TwitchChatMessagePart("text", word));
            }

            return new TwitchChatMessage(sender, parts.ToArray());
        }
    }
}
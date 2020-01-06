namespace TWitcher3.Core.Chat
{
    public class ChatMessageWordResolver : IChatMessageWordResolver
    {
        private readonly IProfanityFilter profanityFilter;

        public ChatMessageWordResolver(IProfanityFilter profanityFilter)
        {
            this.profanityFilter = profanityFilter;
        }

        public string Resolve(string word)
        {
            return profanityFilter.ReplaceIfBad(word) ?? word;
        }
    }
}
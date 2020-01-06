namespace TWitcher3.Core.Chat
{
    public interface IChatMessageEmoteResolver
    {
        IChatEmote Resolve(string emote, bool highResIfPossible = false);
    }
}
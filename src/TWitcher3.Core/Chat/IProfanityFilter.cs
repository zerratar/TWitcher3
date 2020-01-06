namespace TWitcher3.Core.Chat
{
    public interface IProfanityFilter
    {
        void RemoveBadWord(string word);
        void AddBadWord(string word, string replacement = null, string addedBy = null);
        string ReplaceIfBad(string word);
    }
}
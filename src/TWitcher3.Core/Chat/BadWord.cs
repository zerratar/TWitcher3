namespace TWitcher3.Core.Chat
{
    public class BadWord
    {
        public BadWord(string word, string replacement, string addedBy)
        {
            Word = word;
            Replacement = replacement;
            AddedBy = addedBy;
        }

        public string Word { get; }
        public string Replacement { get; }
        public string AddedBy { get; }

        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }
    }
}
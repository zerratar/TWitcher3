using System;
using System.Linq;

namespace TWitcher3.Core.Chat
{
    public class ProfanityFilter : IProfanityFilter
    {
        private readonly IBadWordRepository badWordRepository;

        public ProfanityFilter(IBadWordRepository badWordRepository)
        {
            this.badWordRepository = badWordRepository;
        }

        public void RemoveBadWord(string word)
        {
            var bad = badWordRepository.Find(word);
            if (bad != null)
                badWordRepository.Remove(bad);
        }

        public void AddBadWord(string word, string replacement = null, string addedBy = null)
        {
            if (badWordRepository.Find(word) == null)
                badWordRepository.Store(new BadWord(word, replacement, addedBy));
        }

        public string ReplaceIfBad(string word)
        {
            var badWords = this.badWordRepository.All();
            foreach (var w in badWords)
            {
                var index = word.IndexOf(w.Word, StringComparison.InvariantCultureIgnoreCase);
                if (index >= 0)
                {
                    var replacement = string.IsNullOrEmpty(w.Replacement)
                        ? new string('*', w.Word.Length)
                        : w.Replacement;

                    var badPart = string.Join("", word.Skip(index).Take(w.Word.Length));
                    return word.Replace(badPart, replacement);
                }
            }

            var badWord = this.badWordRepository.Find(word);
            if (badWord == null)
            {
                return null;
            }

            return string.IsNullOrEmpty(badWord.Replacement)
                ? new string('*', word.Length)
                : badWord.Replacement;
        }
    }
}
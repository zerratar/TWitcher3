using System;
using System.Linq;
using TWitcher3.Core.Repositories;

namespace TWitcher3.Core.Chat
{
    public class BadWordRepository : FileBasedRepository<BadWord>, IBadWordRepository
    {
        public BadWordRepository()
            : base("E:\\stream\\badwords.json")
        {
        }

        public BadWord Find(string word)
        {
            var words = All();
            return words.FirstOrDefault(x => x.Word.Equals(word, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Remove(BadWord bad)
        {
            this.Remove(x => x.Word == bad.Word);
        }

        public void Clear(Predicate<BadWord> predicate = null)
        {
            lock (Mutex)
            {
                if (predicate != null)
                {
                    Items.RemoveAll(predicate);
                }
                else
                {
                    this.Items.Clear();
                }
            }
        }
    }
}
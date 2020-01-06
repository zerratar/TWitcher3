using System;
using TWitcher3.Core.Repositories;

namespace TWitcher3.Core.Chat
{
    public interface IBadWordRepository : IRepository<BadWord>
    {
        BadWord Find(string word);
        void Remove(BadWord bad);
        void Clear(Predicate<BadWord> predicate = null);
    }
}
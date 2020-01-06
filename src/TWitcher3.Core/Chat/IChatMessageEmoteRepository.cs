using TWitcher3.Core.Repositories;

namespace TWitcher3.Core.Chat
{
    public interface IChatMessageEmoteRepository : IRepository<IChatEmote>
    {
        IChatEmote Find(string name);
    }
}
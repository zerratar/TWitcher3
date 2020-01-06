using System;
using System.Threading.Tasks;

namespace TWitcher3.Core.Net
{
    public interface ITcpGameClient
    {
        Task ProcessAsync();
        IGameClientSubcription Subscribe(string cmdIdentifier, Action<IGameCommand> onCommand);
    }
}
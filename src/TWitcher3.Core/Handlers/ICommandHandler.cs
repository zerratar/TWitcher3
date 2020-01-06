using System.Threading.Tasks;
using TWitcher3.Core.Net;

namespace TWitcher3.Core.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(IMessageBroadcaster listener, ICommand cmd);
        void Register<TCommandProcessor>(string cmd, params string[] aliases) 
            where TCommandProcessor : ICommandProcessor;        
    }
}
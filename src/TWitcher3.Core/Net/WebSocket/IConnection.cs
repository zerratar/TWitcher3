using System.Threading.Tasks;

namespace TWitcher3.Core.Net.WebSocket
{
    public interface IConnection
    {
        void EnqueueSend<T>(T data);
        Task SendAsync<T>(T data);
        Task<T> ReceiveAsync<T>();
        Task SendAsync(Packet packet);
        void Close();
        bool Closed { get; }
        Task KeepAlive();
    }
}
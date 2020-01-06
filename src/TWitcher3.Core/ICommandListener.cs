using System;

namespace TWitcher3
{
    public interface ICommandListener : IDisposable
    {
        void Start();
        void Stop();
    }
}
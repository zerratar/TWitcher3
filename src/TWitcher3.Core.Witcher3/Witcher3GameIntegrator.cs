using System;
using System.Threading;
using TWitcher3.Core.Games.Witcher3;

namespace TWitcher3.Core.Games
{
    public class Witcher3GameIntegrator : IGameIntegrator, IDisposable
    {
        private readonly IWitcher3 witcher3;
        private readonly Thread processThread;

        private bool disposed;

        public Witcher3GameIntegrator(IWitcher3 witcher3)
        {
            this.witcher3 = witcher3;
            this.processThread = new Thread(ProcessUpdate);
            this.Start();
        }

        public void Start()
        {
            processThread.Start();
        }

        public void Stop()
        {
            processThread.Join();
        }

        private void ProcessUpdate()
        {
            while (!disposed)
            {
                witcher3.Update();
                System.Threading.Thread.Sleep(16);
            }
        }

        public void Dispose()
        {
            if (this.disposed) return;
            this.disposed = true;
            Stop();
        }
    }
}
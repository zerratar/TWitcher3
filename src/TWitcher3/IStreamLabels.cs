using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TWitcher3
{
    public interface IStreamLabels
    {
    }

    public class StreamLabels : IStreamLabels, IDisposable
    {
        private const string StreamLabelsApp = @"C:\Users\kaaru\AppData\Local\Programs\streamlabels\StreamLabels.exe";
        private readonly TimeSpan interval = TimeSpan.FromSeconds(3);
        private readonly Thread checkThread;
        private bool isRunning;
        private bool disposed;

        public StreamLabels()
        {
            this.isRunning = true;
            this.checkThread = new Thread(this.CheckForStreamLabels);
            this.checkThread.Start();            
        }

        private void CheckForStreamLabels()
        {
            while (isRunning)
            {
                var processes = Process.GetProcesses();
                var process = processes.FirstOrDefault(x => x.ProcessName.Contains("StreamLabels"));
                if (process == null || process.HasExited)
                {
                    StartStreamLabelsApp();
                }
                Thread.Sleep(interval);
            }
        }

        private void StartStreamLabelsApp()
        {
            Process.Start(StreamLabelsApp);
        }

        public void Dispose()
        {
            if (disposed) return;
            isRunning = false;
            checkThread.Join();
            disposed = true;
        }
    }
}

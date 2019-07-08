using System;
using System.Diagnostics;

namespace Armoniasoft.ClientDB.Telemetry
{
    public class Timer : ITimer
    {
        private readonly Stopwatch stopwatch;

        public Timer()
        {
            stopwatch = new Stopwatch();
        }

        public TimeSpan Elapsed => stopwatch.Elapsed;

        public void Start()
        {
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }
    }
}

using System;

namespace Armoniasoft.ClientDB.Telemetry
{
    public interface ITimer
    {
        void Start();
        void Stop();

        TimeSpan Elapsed { get; }
    }
}

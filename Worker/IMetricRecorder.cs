using System;

namespace Worker
{
    public interface IMetricRecorder
    {
        void Reset();
        void Report(string action, string key, TimeSpan elapsed);
    }
}
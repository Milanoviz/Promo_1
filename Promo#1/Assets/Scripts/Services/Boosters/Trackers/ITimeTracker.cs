using System;

namespace Services.Boosters.Trackers
{
    public interface ITimeTracker
    {
        TimeSpan LeftTime { get; }
    }
}
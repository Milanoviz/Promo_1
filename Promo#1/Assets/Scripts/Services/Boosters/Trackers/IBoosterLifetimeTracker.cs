using System;
using Services.Boosters.Enums;

namespace Services.Boosters.Trackers
{
    public interface IBoosterLifetimeTracker
    {
        event EventHandler<BoosterType> DataChanged;
        
        BoosterLifetimeType Type { get; }
        bool IsExpired();

        void Initialize();
        void Dispose();
    }
}
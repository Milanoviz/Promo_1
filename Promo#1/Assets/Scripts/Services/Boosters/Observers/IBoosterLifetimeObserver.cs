using System;
using Services.Boosters.Enums;

namespace Services.Boosters.Observers
{
    public interface IBoosterLifetimeObserver
    {
        event EventHandler<BoosterType> BoosterExpired;

        TimeSpan GetBoosterLeftTime(BoosterType boosterType);
        
        void RegisterBooster(BoosterType boosterType, BoosterLifetimeType boosterLifetimeType, string lifeTimeValue);
        void UnregisterBooster(BoosterType boosterType);
    }
}
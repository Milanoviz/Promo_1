using System;

namespace Services.Energy
{
    public interface IEnergyService
    {
        event EventHandler DataChanged;
        
        int CurrentAmount { get; }
        
        void SetEnergyRegenTimeMultiplier(float multiplier);
    }
}
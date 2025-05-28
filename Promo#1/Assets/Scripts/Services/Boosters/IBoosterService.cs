using System;
using Services.Boosters.Enums;

namespace Services.Boosters
{
    public interface IBoosterService
    {
        event EventHandler<BoosterType> BoosterActivated;
        event EventHandler<BoosterType> BoosterDeactivated;

        bool IsActiveBooster(BoosterType boosterType);
        bool CanAffordBooster(BoosterType boosterType);

        void ActivateBooster(BoosterType boosterType);
        void DeactivateBooster(BoosterType boosterType);
    }
}
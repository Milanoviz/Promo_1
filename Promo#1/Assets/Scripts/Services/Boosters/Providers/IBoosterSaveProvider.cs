using Services.Boosters.Enums;

namespace Services.Boosters.Providers
{
    public interface IBoosterSaveProvider
    {
        void AddActiveBoosterData(BoosterType boosterType, long activateDateTimeTicks);
        void RemoveActiveBoosterData(BoosterType boosterType);
    }
}
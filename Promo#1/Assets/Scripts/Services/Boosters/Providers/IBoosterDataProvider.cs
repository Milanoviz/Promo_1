using Services.Boosters.Data;
using Services.Boosters.Enums;

namespace Services.Boosters.Providers
{
    public interface IBoosterDataProvider
    {
        bool TryGetBoosterData(BoosterType boosterType, out BoosterItemData boosterData);
    }
}
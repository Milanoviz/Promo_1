using Services.Boosters.Configs;
using Services.Boosters.Data;
using Services.Boosters.Enums;

namespace Services.Boosters.Providers
{
    public class BoosterDataProvider : IBoosterDataProvider
    {
        private readonly BoosterConfig boosterConfig;

        public BoosterDataProvider(BoosterConfig boosterConfig)
        {
            this.boosterConfig = boosterConfig;
        }

        public bool TryGetBoosterData(BoosterType boosterType, out BoosterItemData boosterData)
        {
            foreach (var itemData in boosterConfig.BoosterItemDataList)
            {
                if (itemData.Type == boosterType)
                {
                    boosterData = itemData;
                    return true;
                }
            }

            boosterData = null;
            return false;
        }
    }
}
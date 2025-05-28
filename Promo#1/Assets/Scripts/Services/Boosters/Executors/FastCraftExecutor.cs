using Services.Boosters.Data;
using Services.Craft;

namespace Services.Boosters.Executors
{
    public class FastCraftExecutor : IBoosterExecutor
    {
        private const int defaultMultiplier = 1;
        
        private ICraftService craftService;
        private BoosterItemData boosterItemData;

        public FastCraftExecutor(BoosterItemData boosterItemData)
        {
            this.boosterItemData = boosterItemData;
        }
        
        private void Inject(ICraftService craftService)
        {
            this.craftService = craftService;
        }
        
        public void Start()
        {
            var energyRegenMultiplier = CalculateMultiplier(boosterItemData.Value);
            craftService.SetCraftTimeMultiplier(energyRegenMultiplier);
        }

        public void Stop()
        {
            craftService.SetCraftTimeMultiplier(defaultMultiplier);
        }

        private float CalculateMultiplier(int value)
        {
            return value / 100f;
        }
    }
}
using Services.Boosters.Data;
using Services.Energy;

namespace Services.Boosters.Executors
{
    public class FastEnergyRegenExecutor : IBoosterExecutor
    {
        private const int defaultMultiplier = 1;
        
        private IEnergyService energyService;
        private BoosterItemData boosterItemData;

        public FastEnergyRegenExecutor(BoosterItemData boosterItemData)
        {
            this.boosterItemData = boosterItemData;
        }

        private void Inject(IEnergyService energyService)
        {
            this.energyService = energyService;
        }
        
        public void Start()
        {
            var energyRegenMultiplier = CalculateMultiplier(boosterItemData.Value);
            energyService.SetEnergyRegenTimeMultiplier(energyRegenMultiplier);
        }

        public void Stop()
        {
            energyService.SetEnergyRegenTimeMultiplier(defaultMultiplier);
        }

        private float CalculateMultiplier(int value)
        {
            return value / 100f;
        }
    }
}
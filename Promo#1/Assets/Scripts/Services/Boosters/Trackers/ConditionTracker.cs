using System;
using Services.Boosters.Enums;
using Services.Condition;
using Services.Condition.Data;

namespace Services.Boosters.Trackers
{
    public class ConditionTracker : IBoosterLifetimeTracker
    {
        public event EventHandler<BoosterType> DataChanged;

        private IConditionService conditionService;
        
        private BoosterType boosterType;
        private string lifetimeValue;
        private IConditionData conditionData;
        
        public BoosterLifetimeType Type { get; }

        public ConditionTracker(BoosterType boosterType, BoosterLifetimeType lifetimeType, string lifetimeValue)
        {
            this.boosterType = boosterType;
            Type = lifetimeType;
            this.lifetimeValue = lifetimeValue;
        }
        
        private void Inject(IConditionService conditionService)
        {
            this.conditionService = conditionService;
        }
        
        public bool IsExpired()
        {
            return conditionService.IsMet(conditionData);
        }

        public void Initialize()
        {
            conditionData = conditionService.ConvertToConditionData(lifetimeValue);
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            //TODO: subscribe to required events
        }

        private void Unsubscribe()
        {
            //TODO: Unsubscribe from required events
        }

        private void Handler(object sender, EventArgs e)
        {
            if (IsExpired())
            {
                OnDataChanged();
            }
        }

        private void OnDataChanged()
        {
            DataChanged?.Invoke(this, boosterType);
        }
    }
}
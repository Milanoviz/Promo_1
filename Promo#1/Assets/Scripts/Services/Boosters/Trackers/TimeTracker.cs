using System;
using Services.Boosters.Enums;
using Services.Ticker;
using UnityEngine;

namespace Services.Boosters.Trackers
{
    public class TimeTracker : IBoosterLifetimeTracker, ITimeTracker
    {
        public event EventHandler<BoosterType> DataChanged;

        private ITickerService tickerService;
        
        private BoosterType boosterType;
        private DateTime endTime;
        
        public BoosterLifetimeType Type { get; }
        public TimeSpan LeftTime => endTime - DateTime.UtcNow;

        public TimeTracker(BoosterType boosterType, BoosterLifetimeType lifetimeType, string value)
        {
            this.boosterType = boosterType;
            Type = lifetimeType;
            SetEndTime(value);
        }
        
        private void Inject(ITickerService tickerService)
        {
            this.tickerService = tickerService;
        }
        
        public bool IsExpired()
        {
            return LeftTime <= TimeSpan.Zero;
        }

        public void Initialize()
        {
            Subscribe();;
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void SetEndTime(string value)
        {
            if (!long.TryParse(value, out var durationTicks))
            {
                Debug.LogError($"Unknown duration value: {value}");
                return;
            }
			
            var dateTimeNow = DateTime.UtcNow;
            endTime = dateTimeNow.AddTicks(durationTicks);
        }

        private void Subscribe()
        {
            tickerService.OnTick += TickHandler;
        }

        private void Unsubscribe()
        {
            tickerService.OnTick -= TickHandler;
        }

        private void TickHandler(object sender, EventArgs e)
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
using System;
using System.Collections.Generic;
using Services.Boosters.Enums;
using Services.Boosters.Factories;
using Services.Boosters.Trackers;
using Services.Ticker;

namespace Services.Boosters.Observers
{
    public class BoosterLifetimeObserver : IBoosterLifetimeObserver
    {
        public event EventHandler<BoosterType> BoosterExpired;
        
        private readonly IBoosterLifetimeTrackerFactory trackerFactory;

        private readonly Dictionary<BoosterType, IBoosterLifetimeTracker> activeTrackes = new();
        private readonly Dictionary<BoosterType, ITimeTracker> activeTimerTrackes = new();

        public BoosterLifetimeObserver(IBoosterLifetimeTrackerFactory trackerFactory)
        {
            this.trackerFactory = trackerFactory;
        }

        public TimeSpan GetBoosterLeftTime(BoosterType boosterType)
        {
            if (!activeTimerTrackes.ContainsKey(boosterType))
            {
                return TimeSpan.Zero;
            }

            return activeTimerTrackes[boosterType]?.LeftTime ?? TimeSpan.Zero;
        }

        public void RegisterBooster(BoosterType boosterType, BoosterLifetimeType boosterLifetimeType, string lifeTimeValue)
        {
            var tracker = trackerFactory.CreateTracker(boosterType, boosterLifetimeType, lifeTimeValue);
            if (tracker == null)
                return;
			
            RegisterTracker(boosterType, tracker);
        }

        public void UnregisterBooster(BoosterType boosterType)
        {
            if (!activeTrackes.ContainsKey(boosterType))
                return;

            UnregisterTracker(boosterType);
        }
        
        private void RegisterTracker(BoosterType boosterType, IBoosterLifetimeTracker tracker)
        {
            AddActiveTracker(boosterType, tracker);
            AddActiveTimerTracker(boosterType, tracker);
        }
        
        private void UnregisterTracker(BoosterType boosterType)
        {
            RemoveActiveTracker(boosterType);
            RemoveActiveTimerTracker(boosterType);
        }

        private void AddActiveTracker(BoosterType boosterType, IBoosterLifetimeTracker tracker)
        {
            tracker.Initialize();
            tracker.DataChanged += TrackedDataChangedHandler;
            activeTrackes[boosterType] = tracker;
        }

        private void AddActiveTimerTracker(BoosterType boosterType, IBoosterLifetimeTracker tracker)
        {
            if (tracker is ITimeTracker timeTracker)
            {
                activeTimerTrackes[boosterType] = timeTracker;
            }
        }
        
        private void RemoveActiveTracker(BoosterType boosterType)
        {
            if (activeTrackes.TryGetValue(boosterType, out var tracker))
            {
                tracker.Dispose();
                tracker.DataChanged -= TrackedDataChangedHandler;
                activeTrackes.Remove(boosterType);
            }
        }
        
        private void RemoveActiveTimerTracker(BoosterType boosterType)
        {
            if (activeTimerTrackes.ContainsKey(boosterType))
            {
                activeTimerTrackes.Remove(boosterType);
            }
        }

        private void CheckState(BoosterType boosterType)
        {
            if (activeTrackes.TryGetValue(boosterType, out var tracker))
            {
                if (tracker.IsExpired())
                {
                    OnBoosterExpired(boosterType);
                }
            }
        }
        
        private void OnBoosterExpired(BoosterType booster)
        {
            BoosterExpired?.Invoke(this, booster);
        }
        
        private void TrackedDataChangedHandler(object sender, BoosterType booster)
        {
            CheckState(booster);
        }
    }
}
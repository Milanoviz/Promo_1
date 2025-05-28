using System;
using System.Collections.Generic;
using Services.Boosters.Enums;
using Services.Boosters.Trackers;
using UnityEngine;

namespace Services.Boosters.Factories
{
    public class BoosterLifetimeTrackerFactory : IBoosterLifetimeTrackerFactory
    {
        private readonly Dictionary<BoosterLifetimeType, Func<BoosterType, BoosterLifetimeType, string, IBoosterLifetimeTracker>> trackerStorage = new()
        {
            { BoosterLifetimeType.Timer, (type, lifetimeType, lifetimeValue) => new TimeTracker(type, lifetimeType, lifetimeValue)},
            { BoosterLifetimeType.Condition, (type, lifetimeType, lifetimeValue) => new ConditionTracker(type, lifetimeType, lifetimeValue)},
        };
        
        public IBoosterLifetimeTracker CreateTracker(BoosterType boosterType, BoosterLifetimeType lifetimeType, string value)
        {
            if (!trackerStorage.TryGetValue(lifetimeType, out var func))
            {
                Debug.LogError($"Tracker for boosterLifetime {lifetimeType} not found");
                return null;
            }

            return func.Invoke(boosterType, lifetimeType, value);
        }
    }
}
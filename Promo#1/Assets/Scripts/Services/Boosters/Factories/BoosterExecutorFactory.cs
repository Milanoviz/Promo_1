using System;
using System.Collections.Generic;
using Services.Boosters.Data;
using Services.Boosters.Enums;
using Services.Boosters.Executors;
using UnityEngine;

namespace Services.Boosters.Factories
{
    public class BoosterExecutorFactory : IBoosterExecutorFactory
    {
        private readonly Dictionary<BoosterType, Func<BoosterItemData, IBoosterExecutor>> executorStorage = new()
        {
            { BoosterType.FastEnergyRegen, data => new FastEnergyRegenExecutor(data)},
            { BoosterType.FastCraft , data => new FastCraftExecutor(data)},
        };
        
        public IBoosterExecutor CreateExecutor(BoosterItemData boosterItemData)
        {
            if (!executorStorage.TryGetValue(boosterItemData.Type, out var func))
            {
                Debug.LogError($"[BoosterExecutorFactory] Executor for booster {boosterItemData.Type} not found");
                return null;
            }

            return func.Invoke(boosterItemData);
        }
    }
}
using System;
using System.Collections.Generic;
using Services.Boosters.Data;
using Services.Boosters.Enums;
using Services.Boosters.Executors;
using Services.Boosters.Factories;
using Services.Boosters.Observers;
using Services.Boosters.Providers;
using Services.Wallet;
using Services.Wallet.Data;
using UnityEngine;

namespace Services.Boosters
{
    public class BoosterService : IBoosterService
    {
        public event EventHandler<BoosterType> BoosterActivated;
        public event EventHandler<BoosterType> BoosterDeactivated;

        private readonly IBoosterDataProvider dataProvider;
        private readonly IBoosterLifetimeObserver lifetimeObserver;
        private readonly IBoosterExecutorFactory executorFactory;
        private readonly IBoosterSaveProvider boosterSaveProvider;
        private readonly IWalletService walletService;
        
        private readonly HashSet<BoosterType> activeBoosters = new();
        private readonly Dictionary<BoosterType, IBoosterExecutor> activeExecutors = new();

        public BoosterService(IBoosterDataProvider dataProvider, IBoosterLifetimeObserver lifetimeObserver, IBoosterExecutorFactory executorFactory, IBoosterSaveProvider boosterSaveProvider, IWalletService walletService)
        {
            this.dataProvider = dataProvider;
            this.lifetimeObserver = lifetimeObserver;
            this.executorFactory = executorFactory;
            this.boosterSaveProvider = boosterSaveProvider;
            this.walletService = walletService;
        }

        public bool IsActiveBooster(BoosterType boosterType)
        {
            return activeBoosters.Contains(boosterType);
        }

        public bool CanAffordBooster(BoosterType boosterType)
        {
            return walletService.GetItemCount(boosterType.ToString()) > 0;
        }

        public void ActivateBooster(BoosterType boosterType)
        {
            if (!CanAffordBooster(boosterType))
            {
                Debug.LogError($"There are not enough booster {boosterType} to activate");
                return;
            }

            if (!dataProvider.TryGetBoosterData(boosterType, out var boosterData))
            {
                Debug.LogError($"BoosterData {boosterType} isn't found");
                return;
            }

            var lifeTimeValue = CalculateLifetimeValue(boosterData);
            var activationDateTime = DateTime.UtcNow;
            
            ActivateBoosterInternal(boosterData.Type, boosterData.LifeTimeType, lifeTimeValue);
            AddSaveData(boosterType, activationDateTime.Ticks);
            WalletDebit(boosterType);
        }

        public void DeactivateBooster(BoosterType boosterType)
        {
            lifetimeObserver.UnregisterBooster(boosterType);
            DeactivateExecutor(boosterType);
            RemoveSaveData(boosterType);
            var isSuccess = activeBoosters.Remove(boosterType);
            if (isSuccess)
            {
                OnBoosterDeactivated(boosterType);
            }
        }

        private void ActivateBoosterInternal(BoosterType boosterType, BoosterLifetimeType lifetimeType, string lifetimeValue)
        {
            lifetimeObserver.RegisterBooster(boosterType, lifetimeType, lifetimeValue);
            ActivateExecutor(boosterType);
            activeBoosters.Add(boosterType);
            OnBoosterActivated(boosterType);
        }

        private void ActivateExecutor(BoosterType boosterType)
        {
            if (activeExecutors.ContainsKey(boosterType))
                return;

            if (!dataProvider.TryGetBoosterData(boosterType, out var boosterData))
                return;

            var executor = executorFactory.CreateExecutor(boosterData);
            if (executor == null)
                return;
			
            executor.Start();
            activeExecutors.Add(boosterType, executor);
        }

        private void DeactivateExecutor(BoosterType boosterType)
        {
            if (!activeExecutors.TryGetValue(boosterType, out var executor))
                return;
            
            executor.Stop();
            activeExecutors.Remove(boosterType);
        }

        private string CalculateLifetimeValue(BoosterItemData boosterData)
        {
            return boosterData.LifeTimeType switch
            {
                BoosterLifetimeType.Timer => CalculateTimerDuration(boosterData),
                _ => boosterData.LifetimeValue
            };
        }
        
        private string CalculateTimerDuration(BoosterItemData boosterData)
        {
            if (!int.TryParse(boosterData.LifetimeValue, out var baseDurationSeconds))
                return string.Empty;

            var baseDurationTicks = baseDurationSeconds * TimeSpan.TicksPerSecond;
            var activeLeftTimeTicks = lifetimeObserver.GetBoosterLeftTime(boosterData.Type).Ticks;
            return (baseDurationTicks + activeLeftTimeTicks).ToString();
        }

        private void AddSaveData(BoosterType boosterType, long activateDateTimeTicks)
        {
            boosterSaveProvider.AddActiveBoosterData(boosterType, activateDateTimeTicks);
        }
        
        private void RemoveSaveData(BoosterType boosterType)
        {
            boosterSaveProvider.RemoveActiveBoosterData(boosterType);
        }
        
        private void WalletDebit(BoosterType boosterType)
        {
            var walletPack = new WalletItemPack();
            walletPack.SetItemAmount(boosterType.ToString(), 1);
            walletService.Debit(walletPack, new Dictionary<string, object>());
        }
        
        private void OnBoosterActivated(BoosterType booster)
        {
            BoosterActivated?.Invoke(this, booster);
        }

        private void OnBoosterDeactivated(BoosterType booster)
        {
            BoosterDeactivated?.Invoke(this, booster);
        }
    }
}
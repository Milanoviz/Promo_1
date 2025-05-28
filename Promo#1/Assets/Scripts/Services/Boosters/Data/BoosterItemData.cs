using System;
using Services.Boosters.Enums;
using UnityEngine;

namespace Services.Boosters.Data
{
    [Serializable]
    public class BoosterItemData
    {
        [SerializeField] private BoosterType type;
        [SerializeField] private int value;
        [SerializeField] private BoosterLifetimeType lifeTimeType;
        [SerializeField] private string lifetimeValue;

        public BoosterItemData(BoosterType type, int value, BoosterLifetimeType lifeTimeType, string lifetimeValue)
        {
            this.type = type;
            this.value = value;
            this.lifeTimeType = lifeTimeType;
            this.lifetimeValue = lifetimeValue;
        }

        public BoosterType Type => type;
        public int Value => value;
        public BoosterLifetimeType LifeTimeType => lifeTimeType;
        public string LifetimeValue => lifetimeValue;
    }
}
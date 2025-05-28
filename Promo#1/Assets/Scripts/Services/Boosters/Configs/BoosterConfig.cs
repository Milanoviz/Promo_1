using System;
using System.Collections.Generic;
using Services.Boosters.Data;
using UnityEngine;

namespace Services.Boosters.Configs
{
    [Serializable]
    public class BoosterConfig
    {
        [SerializeField] private List<BoosterItemData> boosterItemDataList;

        public BoosterConfig(List<BoosterItemData> boosterItemDataList)
        {
            this.boosterItemDataList = boosterItemDataList;
        }

        public List<BoosterItemData> BoosterItemDataList => boosterItemDataList;
    }
}
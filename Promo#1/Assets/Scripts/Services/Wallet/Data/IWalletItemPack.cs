using System;
using System.Collections.Generic;

namespace Services.Wallet.Data
{
    public interface IWalletItemPack
    {
        string Id { get; }
        Dictionary<string, int> Items { get; }
        
        int GetItemAmount(string itemName);
        void SetItemAmount(string itemName, int amount);
    }
}
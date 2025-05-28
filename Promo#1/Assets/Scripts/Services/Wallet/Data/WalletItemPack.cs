using System.Collections.Generic;

namespace Services.Wallet.Data
{
    public class WalletItemPack : IWalletItemPack
    {
        private readonly Dictionary<string, int> items = new();

        public string Id { get; }
        public Dictionary<string, int> Items => items;
        
        public int GetItemAmount(string itemName)
        {
            return Items.TryGetValue(itemName, out var amount) ? amount : 0;
        }

        public void SetItemAmount(string itemName, int amount)
        {
            if (amount == 0)
            {
                Items.Remove(itemName);
                return;
            }

            Items[itemName] = amount;
        }
    }
}
using System.Collections.Generic;
using Services.Wallet.Data;

namespace Services.Wallet
{
    public interface IWalletService
    {
        void Debit(IWalletItemPack walletItemPack, Dictionary<string, object> analyticsParams = null);
        void Payroll(IWalletItemPack walletItemPack, Dictionary<string, object> analyticsParams = null);
        int GetItemCount(string itemName);
    }
}
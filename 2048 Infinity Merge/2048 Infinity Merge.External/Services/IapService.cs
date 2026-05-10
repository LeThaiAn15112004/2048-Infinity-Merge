using _2048_Infinity_Merge.External.Interfaces;

namespace _2048_Infinity_Merge.External.Services;

public class IapService : IIapService
{
    public bool AdsRemoved => throw new NotImplementedException();

    public Task PurchaseRemoveAdsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RestorePurchasesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
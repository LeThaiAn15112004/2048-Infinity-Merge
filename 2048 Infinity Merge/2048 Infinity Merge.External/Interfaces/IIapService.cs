namespace _2048_Infinity_Merge.External.Interfaces;

public interface IIapService{
    bool AdsRemoved { get; }
    Task PurchaseRemoveAdsAsync(CancellationToken cancellationToken = default);
    Task RestorePurchasesAsync(CancellationToken cancellationToken = default);
}
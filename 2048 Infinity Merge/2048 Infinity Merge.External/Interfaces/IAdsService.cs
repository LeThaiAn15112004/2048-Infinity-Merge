namespace _2048_Infinity_Merge.External.Interfaces;

public interface IAdsService{
    bool ShouldShowAds { get; }
    void PrepareInterstitial();
    void ShowInterstitial();
    void ShowRewardedVideoAsync(CancellationToken cancellationToken = default);
}
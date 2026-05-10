using _2048_Infinity_Merge.External.Interfaces;
namespace _2048_Infinity_Merge.External.Services;

public class AdsService : IAdsService
{
    public bool ShouldShowAds => throw new NotImplementedException();

    public void PrepareInterstitial()
    {
        throw new NotImplementedException();
    }

    public void ShowInterstitial()
    {
        throw new NotImplementedException();
    }

    public void ShowRewardedVideoAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
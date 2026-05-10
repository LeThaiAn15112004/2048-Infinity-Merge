using _2048_Infinity_Merge.Domain.Interfaces;
namespace _2048_Infinity_Merge.Domain.Rules;

public class SystemRandom : ISystemRandom
{
    public int Next(int maxExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxExclusive);
        return Random.Shared.Next(maxExclusive);
    }

    public double NextDouble(double maxExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxExclusive);
        return Random.Shared.NextDouble();
    }
}
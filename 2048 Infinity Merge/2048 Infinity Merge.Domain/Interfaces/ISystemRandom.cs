namespace _2048_Infinity_Merge.Domain.Interfaces;



public interface ISystemRandom
{
    double NextDouble(double maxExclusive );
    int Next(int maxExclusive);
}


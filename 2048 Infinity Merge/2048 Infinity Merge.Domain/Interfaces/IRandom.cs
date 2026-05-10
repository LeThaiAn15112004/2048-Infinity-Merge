namespace _2048_Infinity_Merge.Domain.Interfaces;

public interface IRandom{
    double NextDouble();
    int Next(int maxExclusive);
}
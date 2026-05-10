namespace _2048_Infinity_Merge.Domain.Interfaces;

public interface IStorage
{
	/// <summary>Reads a value for <paramref name="key"/>, or <c>null</c> if missing.</summary>
	T? Get<T>(string key);

	void Set<T>(string key, T value);

	void Remove(string key);
}
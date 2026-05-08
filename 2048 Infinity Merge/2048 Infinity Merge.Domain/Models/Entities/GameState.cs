namespace _2048_Infinity_Merge.Domain;

public class GameState
{
	public required Board Board { get; set; }
	public GameMode Mode { get; set; }
	public int Score { get; set; }
	public TimeSpan? RemainingTime { get; set; }
	public bool IsPaused { get; set; }
}

using _2048_Infinity_Merge.Domain;
using _2048_Infinity_Merge.Domain.Models.Entities;

namespace _2048_Infinity_Merge.Domain.Interfaces;

public interface IMoving
{
    /// <summary>Applies 2048 slide+merge for <paramref name="direction"/>; mutates <paramref name="cells"/>.</summary>
    bool TryApplyMove(Tile[,] cells, int n, Direction direction, List<MergeInfo> merges, ref int score);

    bool ApplyAllRowsLeft(Tile[,] cells, int n, List<MergeInfo> merges, ref int score);

    bool ApplyAllRowsRight(Tile[,] cells, int n, List<MergeInfo> merges, ref int score);

    bool ApplyAllColsUp(Tile[,] cells, int n, List<MergeInfo> merges, ref int score);

    bool ApplyAllColsDown(Tile[,] cells, int n, List<MergeInfo> merges, ref int score);

    bool CompressRowLeft(Tile[,] cells, int r, int n, List<MergeInfo> merges, ref int score);

    bool CompressRowRight(Tile[,] cells, int r, int n, List<MergeInfo> merges, ref int score);

    bool CompressColUp(Tile[,] cells, int c, int n, List<MergeInfo> merges, ref int score);

    bool CompressColDown(Tile[,] cells, int c, int n, List<MergeInfo> merges, ref int score);

    bool IsCellEmpty(Tile tile);
}

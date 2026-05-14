using _2048_Infinity_Merge.Domain;
using _2048_Infinity_Merge.Domain.Models.Entities;

namespace _2048_Infinity_Merge.Domain.Interfaces;

/// <summary>Port for pure 2048 rules: board lifecycle, spawn, moves, and valid-move probe.</summary>
public interface IGameEngine
{
    Board CreateBoard(GridSize size);

    /// <summary>Rolls spawn value <b>2</b> or <b>4</b> using <paramref name="rng"/> (see implementation for weights).</summary>
    int RollSpawnTileValue(ISystemRandom rng);

    Board RandomSpawnTile(Board board, ISystemRandom rng);

    MoveResult Move(Board board, Direction direction);

    bool HasAnyValidMove(Board board);
}

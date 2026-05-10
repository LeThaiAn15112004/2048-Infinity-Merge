using System;
using System.Runtime.Intrinsics.X86;
using _2048_Infinity_Merge.Domain;
using _2048_Infinity_Merge.Domain.Interfaces;
using _2048_Infinity_Merge.Domain.Models.Entities;

namespace _2048_Infinity_Merge.Domain.Rules;

public class GameEngine
{
    private const double SpawnTwoProbability = 0.5;

    public Board CreateBoard(GridSize size)
    {
        var edgeLength = (int)size;
        return new Board
        {
            Size = edgeLength,
            Cells = new Tile[edgeLength, edgeLength],
        };
    }

    /// <summary>
    /// Spawn weights: <see cref="SpawnTwoProbability"/> of the [0, 1) interval → tile <b>2</b>, remainder → tile <b>4</b>
    /// (<see cref="ISystemRandom.NextDouble"/> is uniform in [0, 1)).
    /// </summary>
    public static int RollSpawnTileValue(ISystemRandom rng) =>
        rng.NextDouble(1.0) < SpawnTwoProbability ? 2 : 4;

    public Board RandomSpawnTile(Board board, ISystemRandom rng){
        _ = RollSpawnTileValue(rng);
        return board;
    }

    public MoveResult Move(Board board, Direction direction){
        return new MoveResult(Moved: false, Merges: Array.Empty<MergeInfo>(), Score: 0, IsGameOver: false);
    }

    public bool HasAnyValidMove(Board board)
    {
        return true;
    }
}
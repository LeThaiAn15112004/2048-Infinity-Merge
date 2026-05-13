using System;
using _2048_Infinity_Merge.Domain;
using _2048_Infinity_Merge.Domain.Interfaces;
using _2048_Infinity_Merge.Domain.Models.Entities;

namespace _2048_Infinity_Merge.Domain.Rules;

public class GameEngine
{
    private readonly IMoving _moving;

    public GameEngine(IMoving? moving = null)
    {
        _moving = moving ?? new Moving();
    }

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

    /// <summary>
    /// Picks a random empty cell in <see cref="Board.Cells"/> (<c>Tile[,]</c> indexed by row, column in <c>[0, Size)</c>)
    /// and writes a new tile (value from <see cref="RollSpawnTileValue"/>, new <see cref="Guid"/>).
    /// Empty cells are those with <see cref="Tile.Value"/> == 0. If the grid is full, returns <paramref name="board"/> unchanged.
    /// </summary>
    public Board RandomSpawnTile(Board board, ISystemRandom rng)
    {
        ArgumentNullException.ThrowIfNull(board);
        var cells = board.Cells ?? throw new ArgumentException("Board.Cells must be allocated.", nameof(board));
        var n = board.Size;
        if (n <= 0)
            return board;
        if (cells.GetLength(0) != n || cells.GetLength(1) != n)
            throw new ArgumentException("Cells must be a square of length Board.Size on each axis.", nameof(board));

        var emptyCount = 0;
        for (var r = 0; r < n; r++)
        {
            for (var c = 0; c < n; c++)
            {
                if (IsCellEmpty(cells[r, c]))
                    emptyCount++;
            }
        }

        if (emptyCount == 0)
            return board;

        var pickIndex = rng.Next(emptyCount);
        for (var r = 0; r < n; r++)
        {
            for (var c = 0; c < n; c++)
            {
                if (!IsCellEmpty(cells[r, c]))
                    continue;
                if (pickIndex == 0)
                {
                    var value = RollSpawnTileValue(rng);
                    cells[r, c] = new Tile(value, Guid.NewGuid());
                    return board;
                }

                pickIndex--;
            }
        }

        return board;
    }

    private static bool IsCellEmpty(Tile tile) => tile.Value == 0;

    public MoveResult Move(Board board, Direction direction)
    {
        ArgumentNullException.ThrowIfNull(board);
        var cells = board.Cells ?? throw new ArgumentException("Board.Cells must be allocated.", nameof(board));
        var n = board.Size;
        if (n <= 0)
            return new MoveResult(false, Array.Empty<MergeInfo>(), 0, true);
        if (cells.GetLength(0) != n || cells.GetLength(1) != n)
            throw new ArgumentException("Cells must be a square of length Board.Size on each axis.", nameof(board));

        var merges = new List<MergeInfo>();
        var score = 0;
        var moved = _moving.TryApplyMove(cells, n, direction, merges, ref score);

        var isGameOver = !HasAnyValidMove(board);
        var mergeList = merges.Count == 0 ? (IReadOnlyList<MergeInfo>?)Array.Empty<MergeInfo>() : merges;
        return new MoveResult(moved, mergeList, score, isGameOver);
    }

    /// <summary>
    /// Returns <see langword="true"/> if sliding in at least one <see cref="Direction"/> would change the board
    /// (empty cell or merge possible).
    /// </summary>
    public bool HasAnyValidMove(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var cells = board.Cells ?? throw new ArgumentException("Board.Cells must be allocated.", nameof(board));
        var n = board.Size;
        if (n <= 0)
            return false;
        if (cells.GetLength(0) != n || cells.GetLength(1) != n)
            throw new ArgumentException("Cells must be a square of length Board.Size on each axis.", nameof(board));

        foreach (Direction d in Enum.GetValues<Direction>())
        {
            var probe = CloneBoard(board);
            var probeCells = probe.Cells!;
            var scratch = 0;
            var dummy = new List<MergeInfo>();
            if (_moving.TryApplyMove(probeCells, n, d, dummy, ref scratch))
                return true;
        }

        return false;
    }

    private static Board CloneBoard(Board board)
    {
        var n = board.Size;
        var src = board.Cells!;
        var copy = new Tile[n, n];
        for (var r = 0; r < n; r++)
        {
            for (var c = 0; c < n; c++)
                copy[r, c] = src[r, c];
        }

        return new Board { Size = n, Cells = copy };
    }
}

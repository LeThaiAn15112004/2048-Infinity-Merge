using _2048_Infinity_Merge.Domain;
using _2048_Infinity_Merge.Domain.Interfaces;
using _2048_Infinity_Merge.Domain.Models.Entities;

namespace _2048_Infinity_Merge.Domain.Rules;

public sealed class Moving : IMoving
{
    /// <inheritdoc />
    public bool TryApplyMove(Tile[,] cells, int n, Direction direction, List<MergeInfo> merges, ref int score)
    {
        return direction switch
        {
            Direction.Left => ApplyAllRowsLeft(cells, n, merges, ref score),
            Direction.Right => ApplyAllRowsRight(cells, n, merges, ref score),
            Direction.Up => ApplyAllColsUp(cells, n, merges, ref score),
            Direction.Down => ApplyAllColsDown(cells, n, merges, ref score),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
    }

    public bool ApplyAllRowsLeft(Tile[,] cells, int n, List<MergeInfo> merges, ref int score)
    {
        var any = false;
        for (var r = 0; r < n; r++)
            any |= CompressRowLeft(cells, r, n, merges, ref score);
        return any;
    }

    public bool ApplyAllRowsRight(Tile[,] cells, int n, List<MergeInfo> merges, ref int score)
    {
        var any = false;
        for (var r = 0; r < n; r++)
            any |= CompressRowRight(cells, r, n, merges, ref score);
        return any;
    }

    public bool ApplyAllColsUp(Tile[,] cells, int n, List<MergeInfo> merges, ref int score)
    {
        var any = false;
        for (var c = 0; c < n; c++)
            any |= CompressColUp(cells, c, n, merges, ref score);
        return any;
    }

    public bool ApplyAllColsDown(Tile[,] cells, int n, List<MergeInfo> merges, ref int score)
    {
        var any = false;
        for (var c = 0; c < n; c++)
            any |= CompressColDown(cells, c, n, merges, ref score);
        return any;
    }

    public bool CompressRowLeft(Tile[,] cells, int r, int n, List<MergeInfo> merges, ref int score)
    {
        var before = new Tile[n];
        for (var c = 0; c < n; c++)
            before[c] = cells[r, c];

        var line = new List<(Tile t, int c)>();
        for (var c = 0; c < n; c++)
        {
            if (!IsCellEmpty(cells[r, c]))
                line.Add((cells[r, c], c));
        }

        var outs = new List<Tile>();
        var i = 0;
        while (i < line.Count)
        {
            if (i + 1 < line.Count && line[i].t.Value == line[i + 1].t.Value)
            {
                var v = line[i].t.Value * 2;
                var nid = Guid.NewGuid();
                score += v;
                var tc = outs.Count;
                merges.Add(new MergeInfo(line[i].t.Id, line[i].c, r, tc, r, true, v));
                merges.Add(new MergeInfo(line[i + 1].t.Id, line[i + 1].c, r, tc, r, true, v));
                outs.Add(new Tile(v, nid));
                i += 2;
            }
            else
            {
                var tc = outs.Count;
                if (line[i].c != tc)
                    merges.Add(new MergeInfo(line[i].t.Id, line[i].c, r, tc, r, false, line[i].t.Value));
                outs.Add(line[i].t);
                i++;
            }
        }

        for (var c = 0; c < n; c++)
            cells[r, c] = c < outs.Count ? outs[c] : default;

        for (var c = 0; c < n; c++)
        {
            if (before[c].Value != cells[r, c].Value || before[c].Id != cells[r, c].Id)
                return true;
        }

        return false;
    }

    public bool CompressRowRight(Tile[,] cells, int r, int n, List<MergeInfo> merges, ref int score)
    {
        var before = new Tile[n];
        for (var c = 0; c < n; c++)
            before[c] = cells[r, c];

        var line = new List<(Tile t, int c)>();
        for (var c = n - 1; c >= 0; c--)
        {
            if (!IsCellEmpty(cells[r, c]))
                line.Add((cells[r, c], c));
        }

        var outs = new List<Tile>();
        var i = 0;
        while (i < line.Count)
        {
            if (i + 1 < line.Count && line[i].t.Value == line[i + 1].t.Value)
            {
                var v = line[i].t.Value * 2;
                var nid = Guid.NewGuid();
                score += v;
                var tc = n - 1 - outs.Count;
                merges.Add(new MergeInfo(line[i].t.Id, line[i].c, r, tc, r, true, v));
                merges.Add(new MergeInfo(line[i + 1].t.Id, line[i + 1].c, r, tc, r, true, v));
                outs.Add(new Tile(v, nid));
                i += 2;
            }
            else
            {
                var tc = n - 1 - outs.Count;
                if (line[i].c != tc)
                    merges.Add(new MergeInfo(line[i].t.Id, line[i].c, r, tc, r, false, line[i].t.Value));
                outs.Add(line[i].t);
                i++;
            }
        }

        for (var c = 0; c < n; c++)
            cells[r, c] = default;
        for (var k = 0; k < outs.Count; k++)
            cells[r, n - 1 - k] = outs[k];

        for (var c = 0; c < n; c++)
        {
            if (before[c].Value != cells[r, c].Value || before[c].Id != cells[r, c].Id)
                return true;
        }

        return false;
    }

    public bool CompressColUp(Tile[,] cells, int c, int n, List<MergeInfo> merges, ref int score)
    {
        var before = new Tile[n];
        for (var r = 0; r < n; r++)
            before[r] = cells[r, c];

        var line = new List<(Tile t, int r)>();
        for (var r = 0; r < n; r++)
        {
            if (!IsCellEmpty(cells[r, c]))
                line.Add((cells[r, c], r));
        }

        var outs = new List<Tile>();
        var i = 0;
        while (i < line.Count)
        {
            if (i + 1 < line.Count && line[i].t.Value == line[i + 1].t.Value)
            {
                var v = line[i].t.Value * 2;
                var nid = Guid.NewGuid();
                score += v;
                var tr = outs.Count;
                merges.Add(new MergeInfo(line[i].t.Id, c, line[i].r, c, tr, true, v));
                merges.Add(new MergeInfo(line[i + 1].t.Id, c, line[i + 1].r, c, tr, true, v));
                outs.Add(new Tile(v, nid));
                i += 2;
            }
            else
            {
                var tr = outs.Count;
                if (line[i].r != tr)
                    merges.Add(new MergeInfo(line[i].t.Id, c, line[i].r, c, tr, false, line[i].t.Value));
                outs.Add(line[i].t);
                i++;
            }
        }

        for (var r = 0; r < n; r++)
            cells[r, c] = r < outs.Count ? outs[r] : default;

        for (var r = 0; r < n; r++)
        {
            if (before[r].Value != cells[r, c].Value || before[r].Id != cells[r, c].Id)
                return true;
        }

        return false;
    }

    public bool CompressColDown(Tile[,] cells, int c, int n, List<MergeInfo> merges, ref int score)
    {
        var before = new Tile[n];
        for (var r = 0; r < n; r++)
            before[r] = cells[r, c];

        var line = new List<(Tile t, int r)>();
        for (var r = n - 1; r >= 0; r--)
        {
            if (!IsCellEmpty(cells[r, c]))
                line.Add((cells[r, c], r));
        }

        var outs = new List<Tile>();
        var i = 0;
        while (i < line.Count)
        {
            if (i + 1 < line.Count && line[i].t.Value == line[i + 1].t.Value)
            {
                var v = line[i].t.Value * 2;
                var nid = Guid.NewGuid();
                score += v;
                var tr = n - 1 - outs.Count;
                merges.Add(new MergeInfo(line[i].t.Id, c, line[i].r, c, tr, true, v));
                merges.Add(new MergeInfo(line[i + 1].t.Id, c, line[i + 1].r, c, tr, true, v));
                outs.Add(new Tile(v, nid));
                i += 2;
            }
            else
            {
                var tr = n - 1 - outs.Count;
                if (line[i].r != tr)
                    merges.Add(new MergeInfo(line[i].t.Id, c, line[i].r, c, tr, false, line[i].t.Value));
                outs.Add(line[i].t);
                i++;
            }
        }

        for (var r = 0; r < n; r++)
            cells[r, c] = default;
        for (var k = 0; k < outs.Count; k++)
            cells[n - 1 - k, c] = outs[k];

        for (var r = 0; r < n; r++)
        {
            if (before[r].Value != cells[r, c].Value || before[r].Id != cells[r, c].Id)
                return true;
        }

        return false;
    }

    public bool IsCellEmpty(Tile tile) => tile.Value == 0;
}

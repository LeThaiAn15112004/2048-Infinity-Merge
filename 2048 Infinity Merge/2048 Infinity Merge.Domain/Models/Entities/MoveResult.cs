using System;
using System.Collections.Generic;
using System.Text;

namespace _2048_Infinity_Merge.Domain.Models.Entities
{
    public record MoveResult(
        bool Moved, 
        IReadOnlyList<MergeInfo>? Merges, 
        int Score, 
        bool IsGameOver
    );
}

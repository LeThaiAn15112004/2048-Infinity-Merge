using System;
using System.Collections.Generic;
using System.Text;

namespace _2048_Infinity_Merge.Domain.Models.Entities
{
    public record MergeInfo(
        Guid TileId, 
        int FromCol, 
        int FromRow, 
        int ToCol, 
        int ToRow, 
        bool IsMerged, 
        int ValueAfter
    );
}

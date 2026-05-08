using System;
using System.Collections.Generic;
using System.Text;

namespace _2048_Infinity_Merge.Domain.Models.Entities
{
    public class MergeInfo
    {
        public Guid TileId { get; set; }
        public int FromCol { get; set; }
        public int FromRow { get; set; }
        public int ToCol { get; set; }
        public int ToRow { get; set; }
        public int Value { get; set; }
    }
}

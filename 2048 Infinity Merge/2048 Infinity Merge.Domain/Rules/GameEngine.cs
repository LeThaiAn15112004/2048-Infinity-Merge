using System;
using _2048_Infinity_Merge.Domain.Interfaces;
using _2048_Infinity_Merge.Domain.Models.Entities;

namespace _2048_Infinity_Merge.Domain.Rules;

public class GameEngine{
    public Board CreateBoard(GridSize size){
        return new Board();
    }

    public Board RandomSpawnTile(Board board, IRandom rng){
        return new Board();
    }

    public MoveResult Move(Board board, Direction direction){
        return new MoveResult(Moved: false, Merges: Array.Empty<MergeInfo>(), Score: 0, IsGameOver: false);
    }

    public bool HasAnyValidMove(Board board)
    {
        return true;
    }
}
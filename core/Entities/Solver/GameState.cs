using System.Collections.Generic;
using core;

public class GameState
{
    public List<V2Int> moves;
    public V2Int playerPosition;
    public int activeBoardIndex;

    //TODO: Probably Conditions instead of 1 by 1, could make use of same reference for conditions
    public int holesUsed;

    public GameState(List<V2Int> moves, V2Int playerPosition, int activeBoardIndex, int holesUsed)
    {
        this.moves = new List<V2Int>(moves);
        this.playerPosition = playerPosition;
        this.activeBoardIndex = activeBoardIndex;
        this.holesUsed = holesUsed;
    }
}
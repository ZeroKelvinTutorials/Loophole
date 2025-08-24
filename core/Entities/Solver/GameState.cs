using System.Collections.Generic;
using core;

public class GameState
{
    public List<V2Int> moves;
    public V2Int playerPosition;
    public int activeBoardIndex;

    public GameState parent = null;
    public List<GameState> children = new List<GameState>();


    //TODO: Probably Conditions instead of 1 by 1, could make use of same reference for conditions
    public int holesUsed;

    public GameState(List<V2Int> moves, V2Int playerPosition, int activeBoardIndex, int holesUsed)
    {
        this.moves = new List<V2Int>(moves);
        this.playerPosition = playerPosition;
        this.activeBoardIndex = activeBoardIndex;
        this.holesUsed = holesUsed;
    }

    //Returns false if the state has been visitted before
    //We need to evaluate if it has been visitted, BUT we have less holes used.........
    public void UpdateState(V2Int move, int activeBoardIndex, V2Int position)
    {
        moves.Add(move);
        this.activeBoardIndex = activeBoardIndex;
        this.playerPosition = position;
    }

    public bool IsWinState(Level level)
    {
        return (level.boards[this.activeBoardIndex].goal != null && level.boards[this.activeBoardIndex].goal.position == this.playerPosition);
    }

    public List<GameState> GetChildren()
    {
        List<GameState> allChildren = new List<GameState>();

        foreach (GameState child in children)
        {
            allChildren.Add(child);
            allChildren.AddRange(child.GetChildren());
        }

        return allChildren;
    }
}
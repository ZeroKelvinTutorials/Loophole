using System;
using core;

public class SolverSubscriber
{
    GameState gameState;

    public void Subscribe(Level level, GameState gameState)
    {
        this.gameState = gameState;
        level.OnHoleEntered += OnHoleEntered;
    }
    public void UnSubscribe(Level level)
    {
        level.OnHoleEntered -= OnHoleEntered;
    }

    void OnHoleEntered(V2Int holePosition)
    {
        gameState.holesUsed++;
    }
}
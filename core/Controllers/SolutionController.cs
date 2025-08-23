using System;
using System.Collections.Generic;
using System.Diagnostics;
using core;

public static class SolutionController
{
    static SolverSubscriber solverSubscriber = new SolverSubscriber();

    public static bool debug = false;

    public static bool FindShortestPath(Level level, out GameState winState)
    {
        GameState initialState = new GameState(new List<V2Int>(), level.player.position, level.activeBoardIndex, 0);
        winState = initialState;

        List<GameState> visitedStates = new List<GameState> { initialState };

        List<GameState> activeStates = new List<GameState> { initialState };
        while (activeStates.Count > 0)
        {
            List<GameState> validFutueStates = new List<GameState>();

            foreach (GameState presentState in activeStates)
            {
                foreach (V2Int direction in V2Int.CardinalDirections())
                {
                    //set level conditions to match presentState
                    level.player.position = presentState.playerPosition;
                    level.activeBoardIndex = presentState.activeBoardIndex;

                    GameState futureState = new GameState(presentState.moves, presentState.playerPosition, presentState.activeBoardIndex, presentState.holesUsed);

                    //Make the move, we subscribe to be able to modify our gamestate conditions etc.. i.e. hole entered count
                    solverSubscriber.Subscribe(level, futureState);
                    PlayerController.TryMove(level.player, level, direction);
                    solverSubscriber.UnSubscribe(level);


                    //Populate future state with new level state
                    futureState.playerPosition = level.player.position;
                    futureState.moves.Add(direction);
                    futureState.activeBoardIndex = level.activeBoardIndex;

                    //Check future state against current and past states to see if it already exists
                    bool dupe = false;
                    if (futureState.playerPosition == presentState.playerPosition && futureState.activeBoardIndex == presentState.activeBoardIndex)
                    {
                        //Position didn't change, this is a non-valid future state since its not new
                        dupe = true;
                    }
                    else
                    {
                        foreach (GameState pastState in visitedStates)
                        {
                            if (pastState.playerPosition == futureState.playerPosition && pastState.activeBoardIndex == futureState.activeBoardIndex)
                            {
                                //We can safely assume if the position is visitted already the futurestate is longer or similar in moves so we consider it dupe
                                dupe = true;
                                break;
                            }
                        }
                    }
                    if (dupe)
                    {
                        continue;
                    }

                    //Check win state
                    if (level.boards[futureState.activeBoardIndex].goal != null && level.boards[futureState.activeBoardIndex].goal.position == futureState.playerPosition)
                    {
                        if (debug)
                        {
                            Console.WriteLine("Solution Found");
                            foreach (V2Int move in futureState.moves)
                            {
                                Console.WriteLine(move.ToString());
                            }
                        }

                        winState = futureState;
                        return true;
                    }

                    validFutueStates.Add(futureState);
                    visitedStates.Add(futureState);
                }

            }

            activeStates = new List<GameState>(validFutueStates);
        }

        //No solution found
        return false;
    }
}
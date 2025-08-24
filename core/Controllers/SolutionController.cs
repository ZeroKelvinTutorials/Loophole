using System;
using System.Collections.Generic;
using System.Diagnostics;
using core;

public static class SolutionController
{
    static SolverSubscriber solverSubscriber = new SolverSubscriber();

    public static bool debug = false;

    //Dupes are considered on a "this position has been visitted by another state"
    public static bool FindShortestPath(Level OGlevel, out GameState winState)
    {
        Level level = OGlevel.Clone();

        GameState initialState = new GameState(new List<V2Int>(), level.player.position, level.activeBoardIndex, 0);
        winState = initialState;

        List<GameState> visitedStates = new List<GameState> { initialState };

        List<GameState> activeStates = new List<GameState> { initialState };
        while (activeStates.Count > 0)
        {
            List<GameState> validFutureStates = new List<GameState>();

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

                    validFutureStates.Add(futureState);
                    visitedStates.Add(futureState);
                }

            }

            activeStates = new List<GameState>(validFutureStates);
        }

        //No solution found
        return false;
    }

    //We check that there are no viable solutions with less holes used than the specified ones
    //No dupe check, we only check against the states in the current states past.
    public static bool VerifySolution(Level OGlevel, int holesUsed)
    {
        // Console.WriteLine("Verifying");
        Level level = OGlevel.Clone();

        bool solutionFound = false;

        GameState initialState = new GameState(new List<V2Int>(), level.player.position, level.activeBoardIndex, 0);

        List<GameState> visittedStates = new List<GameState> { initialState };
        List<GameState> activeStates = new List<GameState> { initialState };

        int turn = 0;
        while (activeStates.Count > 0)
        {
            turn++;
            // Console.WriteLine("Turn " + turn + ": " + activeStates.Count + " active states");
            List<GameState> validFutureStates = new List<GameState>();
            //These are states that will be overwritten by a new one due to less holes used
            //Them and their children states will be destroyed at the end of the iteration
            //Them and their children will be removed from validFutureStates and visittedStates
            //No trace of them and their children will go to active states
            List<GameState> deadStates = new List<GameState>();

            foreach (GameState presentState in activeStates)
            {
                foreach (V2Int direction in V2Int.CardinalDirections())
                {
                    //set level conditions to match presentState
                    level.player.position = presentState.playerPosition;
                    level.activeBoardIndex = presentState.activeBoardIndex;

                    //Initialize future state, we need it so we can subscribe events that alter it while listening to player move
                    GameState futureState = new GameState(presentState.moves, presentState.playerPosition, presentState.activeBoardIndex, presentState.holesUsed);

                    //Make the move, we subscribe to be able to modify our gamestate conditions etc.. i.e. hole entered count
                    solverSubscriber.Subscribe(level, futureState);
                    PlayerController.TryMove(level.player, level, direction);
                    solverSubscriber.UnSubscribe(level);

                    futureState.UpdateState(direction, level.activeBoardIndex, level.player.position);

                    //Check if a past state exists.
                    if (futureState.playerPosition == presentState.playerPosition && futureState.activeBoardIndex == presentState.activeBoardIndex)
                    {
                        //Position didn't change, this is a non-valid future state since its not new
                        continue;
                    }

                    int taggedOthers = 0;
                    bool dupe = false;
                    foreach (GameState otherState in visittedStates)
                    {
                        if (futureState.playerPosition == otherState.playerPosition && futureState.activeBoardIndex == otherState.activeBoardIndex)
                        {
                            //We got somewhere in less holes than in the past, right now we're checking for hole condition so we will overwrite those since there's a "worse" way of getting there
                            if (otherState.holesUsed > futureState.holesUsed && !deadStates.Contains(otherState))
                            {
                                taggedOthers++;
                                deadStates.Add(otherState);
                            }
                            else
                            {
                                dupe = true;
                            }
                        }
                    }
                    if (dupe)
                    {
                        continue;
                    }
                    //NOTE: We can get dupe true and multiple deadStates, what that means is that we already went through this in the same turn.
                    //ie. 0,0 we all move to another location. through a hle, then on the next turn we all move back in through the hole... or something like that
                    // if (taggedOthers > 0)
                    // {
                    //     // Console.WriteLine(dupe + " " + taggedOthers + " " + futureState.activeBoardIndex + " " + futureState.playerPosition.ToString() + " " + turn + level.boards[futureState.activeBoardIndex].GetTile(futureState.playerPosition).tileType.ToString());
                    // }



                    //First we check through 
                    //If it does, and the holesused are less or equal, then we invalidate current futureState
                    //If it does, BUT the holesUsed are more, then we tag that state as invalid and we use the current one

                    //Check win state
                    if (futureState.IsWinState(level))
                    {
                        // Console.WriteLine("Win state " + futureState.holesUsed + " " + holesUsed);
                        if (futureState.holesUsed < holesUsed)
                        {
                            return false;
                        }

                        solutionFound = true;
                    }
                    else
                    {
                        //We only want to keep non-win maps alive
                        presentState.children.Add(futureState);
                        futureState.parent = presentState;
                        visittedStates.Add(futureState);
                        validFutureStates.Add(futureState);
                    }
                }
            }

            //Remove all dead states and their children from validFuturetates, visittedStates
            foreach (GameState deadState in deadStates)
            {
                List<GameState> nestedChildren = deadState.GetChildren();

                // Console.WriteLine("Dead State with " + nestedChildren.Count + "children");

                deadState.parent.children.Remove(deadState);
                foreach (GameState child in nestedChildren)
                {
                    visittedStates.Remove(child);
                    validFutureStates.Remove(child);
                }
                visittedStates.Remove(deadState);
                validFutureStates.Remove(deadState);
            }

            activeStates = new List<GameState>(validFutureStates);
        }
        // Console.WriteLine("All states visitted");
        return solutionFound;
    }
}
using System;

namespace core
{
    public static class PlayerController
    {
        public static bool TryMove(Player player, Level level, V2Int direction)
        {
            Board board = level.boards[level.activeBoardIndex];
            V2Int playerPosition = level.player.position;

            V2Int targetPosition = playerPosition + direction;
            targetPosition = targetPosition.Loop(board.width, board.height);


            Tile targetTile = board.GetTile(targetPosition);
            if (targetTile == null)
            {
                //error
                return false;
            }

            switch (targetTile.tileType)
            {
                case TileType.Ground:

                    //If theres a box
                    if (board.boxes.ContainsKey(targetPosition))
                    {
                        //First we check if were on a rock or box, if we are then we move normally
                        if (board.GetTile(player.position).tileType == TileType.Rock || board.boxes.ContainsKey(player.position))
                        {
                            player.position = targetPosition;
                            level.player.position = targetPosition;
                            player.direction = V2Int.Zero;
                            player.InvokeOnMove(targetPosition);
                            break;
                        }
                        //If thats not the cse we check if we can move the box.
                        else if (BoxUtility.CanMove(board, board.boxes[targetPosition], direction))
                        {
                            V2Int newBoxPosition = (targetPosition + direction).Loop(board.width, board.height);

                            //Move box
                            Box box = board.boxes[targetPosition];
                            if (board.GetTile(newBoxPosition).tileType == TileType.Hole)
                            {
                                //Get next board
                                int nextBoardIndex = level.activeBoardIndex + 1;
                                if (nextBoardIndex == level.boards.Length)
                                {
                                    nextBoardIndex = 0;
                                }
                                Board nextBoard = level.boards[nextBoardIndex];

                                board.boxes.Remove(targetPosition);

                                box.position = newBoxPosition;
                                nextBoard.boxes.Add(newBoxPosition, box);

                                box.InvokeOnFall(newBoxPosition);
                            }
                            else
                            {
                                board.boxes.Remove(targetPosition);

                                box.position = newBoxPosition;
                                board.boxes.Add(newBoxPosition, box);

                                box.InvokeOnMove(newBoxPosition);
                            }
                            //we dont break here so the player can be moved normally in next step
                        }
                        //Otherwise the box stops our movement.
                        else
                        {
                            return false;
                        }
                    }

                    //If no box we move normally 
                    player.position = targetPosition;
                    level.player.position = targetPosition;
                    player.direction = V2Int.Zero;
                    player.InvokeOnMove(targetPosition);
                    break;
                case TileType.Rock:
                    //if we are on a rock or a box, we can move between rocks and boxes
                    if (board.boxes.ContainsKey(player.position) || board.GetTile(player.position).tileType == TileType.Rock)
                    {
                        player.position = targetPosition;
                        level.player.position = targetPosition;
                        player.direction = V2Int.Zero;
                        player.InvokeOnMove(targetPosition);
                    }
                    else
                    {
                        player.direction = V2Int.Zero;
                        return false;
                    }
                    break;
                case TileType.Ice:
                    //TODO: Add box logic although probably wont implement for this stage
                    player.position = targetPosition;
                    level.player.position = targetPosition;
                    player.direction = direction;
                    player.InvokeOnMove(targetPosition);
                    //Todo. recursively call try move periodically...
                    break;
                case TileType.Hole:
                    player.position = targetPosition;

                    //Change board downwards
                    int boardIndex = level.activeBoardIndex + 1;
                    if (boardIndex == level.boards.Length)
                    {
                        boardIndex = 0;
                    }
                    level.activeBoardIndex = boardIndex;
                    level.player.position = targetPosition;
                    level.InvokeOnHoleEntered(targetPosition, level.boards[boardIndex]);
                    break;
                default:
                    //error
                    return false;
            }

            if (board.goal != null && board.goal.position == player.position)
            {
                level.InvokeOnWin(player.position);
            }

            return true;
            //Try Move recursively if direction not 0
        }
    }
}

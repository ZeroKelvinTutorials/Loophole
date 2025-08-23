using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace core
{
    public static class LevelFactory
    {

        //Parses a text file into a level object
        public static Level CreateLevel(string[] lines)
        {
            //Assert that all lines are same length OR 0 length to keep width/height consistency
            List<Board> boards = new List<Board>();
            Board board = null;
            int activeBoardIndex = 0;
            //Note, we could have the initial board be another than 0 index (basically have player not start at the top)
            int currentBoardIndex = 0;
            int currentHeightIndex = 0;

            Player player = null;
            bool hasPlayer = false;

            //Parse lines into boards
            foreach (string line in lines)
            {
                //If we reached a white space make sure to add the board and reset the current one
                if (String.IsNullOrWhiteSpace(line))
                {
                    if (board != null)
                    {
                        boards.Add((Board)board.Clone());
                        if (hasPlayer)
                        {
                            activeBoardIndex = currentBoardIndex;
                        }
                        board = null;
                        hasPlayer = false;
                        currentBoardIndex++;
                        currentHeightIndex = 0;
                    }
                    continue;
                }

                if (board == null)
                {
                    Console.WriteLine("New board");
                    board = new Board();
                }

                for (int x = 0; x < line.Length; x++)
                {
                    board.width = x + 1;

                    V2Int position = new V2Int(x, currentHeightIndex);
                    switch (line[x])
                    {
                        case '0':
                        case 'X':
                            board.tiles.Add(position, new Tile(TileType.Ground));
                            break;
                        case '1':
                            board.tiles.Add(position, new Tile(TileType.Rock));
                            break;
                        case 'H':
                            board.tiles.Add(position, new Tile(TileType.Hole));
                            break;
                        case 'P':
                            board.tiles.Add(position, new Tile(TileType.Ground));

                            //if a player already exists, quit process
                            if (player != null)
                            {
                                return null;
                            }

                            player = new Player();
                            player.position = position;
                            player.direction = V2Int.Zero;

                            hasPlayer = true;

                            break;
                        case 'G':
                            board.tiles.Add(position, new Tile(TileType.Ground));
                            board.goal = new Goal();
                            board.goal.position = position;
                            break;
                        case 'B':
                            board.tiles.Add(position, new Tile(TileType.Ground));
                            Box box = new Box();
                            box.position = position;
                            board.boxes.Add(position, box);
                            break;
                        default:
                            return null;

                    }
                }
                currentHeightIndex++;
                board.height = currentHeightIndex;
            }

            //Add last board
            if (board != null)
            {
                boards.Add((Board)board.Clone());
                if (hasPlayer)
                {
                    activeBoardIndex = currentBoardIndex;
                }
                Console.WriteLine("Last board");
            }

            //TODO: ASSERT THAT A PLAYER EXISTS AND ANYTHING ELSE IMPORTANT, or maybe in level constructor?

            //Create Level from boards
            return new Level(boards.ToArray(), activeBoardIndex, player);
        }
    }
}
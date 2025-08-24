using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace core
{
    public static class LevelFactory
    {
        static Random random = new Random();

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


        //TODO: Allow aligned holes when they dont form a loop
        public static Level GenerateRandomLevel(int sizeX, int sizeY, int boardCount, int wallCount, int holeCount)
        {
            Board[] boards = new Board[boardCount];

            //Initialize boards with ground everywhere
            for (int i = 0; i < boardCount; i++)
            {
                Board board = new Board();
                board.width = sizeX;
                board.height = sizeY;
                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        board.tiles.Add(new V2Int(x, y), new Tile(TileType.Ground));
                    }
                }
                boards[i] = board;
            }

            //Initialize record of placedPositions
            Dictionary<int, List<V2Int>> placedPositions = new Dictionary<int, List<V2Int>>();
            for (int i = 0; i < boardCount; i++)
            {
                placedPositions.Add(i, new List<V2Int>());
            }

            //Place player
            Tuple<int, V2Int> validPosition = GetValidPosition();
            placedPositions[validPosition.Item1].Add(validPosition.Item2);
            Player player = new Player();
            player.position = validPosition.Item2;
            int activeBoardIndex = validPosition.Item1;

            //Place goal
            validPosition = GetValidPosition();
            placedPositions[validPosition.Item1].Add(validPosition.Item2);
            Goal goal = new Goal();
            goal.position = validPosition.Item2;
            boards[validPosition.Item1].goal = goal;

            //Place Walls
            for (int i = 0; i < wallCount; i++)
            {
                validPosition = GetValidPosition();
                placedPositions[validPosition.Item1].Add(validPosition.Item2);
                boards[validPosition.Item1].GetTile(validPosition.Item2).tileType = TileType.Rock;
            }

            //Place Holes
            for (int i = 0; i < holeCount; i++)
            {
                //CHECK THAT NO HOLE EXISTS ABOVE OR BELOW
                validPosition = GetValidPosition(true);
                placedPositions[validPosition.Item1].Add(validPosition.Item2);
                boards[validPosition.Item1].GetTile(validPosition.Item2).tileType = TileType.Hole;
            }


            return new Level(boards.ToArray(), activeBoardIndex, player);

            Tuple<int, V2Int> GetValidPosition(bool hole = false)
            {
                while (true)
                {
                    int randomBoardIndex = random.Next(0, boards.Length);
                    V2Int position = new V2Int(random.Next(0, sizeX), random.Next(0, sizeY));

                    //Validate that there arent holes above or below
                    if (hole)
                    {
                        if (boards[GetAbove(randomBoardIndex, boards.Length)].GetTile(position).tileType == TileType.Hole)
                        {
                            continue;
                        }
                        if (boards[GetBelow(randomBoardIndex, boards.Length)].GetTile(position).tileType == TileType.Hole)
                        {
                            continue;
                        }
                    }
                    if (!placedPositions[randomBoardIndex].Contains(position))
                    {
                        return new Tuple<int, V2Int>(randomBoardIndex, position);
                    }
                }
            }
        }
        static int GetAbove(int index, int count)
        {
            int above = index - 1;
            if (above < 0)
            {
                above = count - 1;
            }
            return above;
        }
        static int GetBelow(int index, int count)
        {
            int below = index + 1;
            if (below >= count)
            {
                below = 0;
            }
            return below;
        }
    }
}
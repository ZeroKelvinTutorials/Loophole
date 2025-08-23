using System.Text;
using core;

public static class Program
{
    public static void Main()
    {
        string[] lines = File.ReadAllLines("Levels/Level0.txt");

        Level level = LevelFactory.CreateLevel(lines);
        Print(level);

        while (true)
        {
            V2Int direction = GetInput();

            PlayerController.TryMove(level.player, level, direction.InvertYAxis());
            Print(level);
        }
    }

    static void Print(Board board, Level level)
    {
        for (int y = 0; y < board.height; y++)
        {
            StringBuilder line = new StringBuilder();

            for (int x = 0; x < board.width; x++)
            {

                V2Int pos = new V2Int(x, y);
                if (level.player.position == pos)
                {
                    line.Append("P");
                }
                else if (board.boxes.ContainsKey(pos))
                {
                    line.Append("B");
                }
                else
                {
                    switch (board.GetTile(pos).tileType)
                    {
                        case TileType.Ground:
                            line.Append("0");
                            break;
                        case TileType.Hole:
                            line.Append("H");
                            break;
                        case TileType.Ice:
                            line.Append("I");
                            break;
                        case TileType.Rock:
                            line.Append("1");
                            break;
                    }
                }
            }
            Console.WriteLine(line.ToString());
        }
        Console.WriteLine();
    }

    static void Print(Level level)
    {
        Board activeBoard = level.boards[level.activeBoardIndex];
        Print(activeBoard, level);

    }

    static V2Int GetInput()
    {
        bool directionFound = false;
        while (!directionFound)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return V2Int.Left;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return V2Int.Down;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return V2Int.Up;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    return V2Int.Right;
            }
        }
        return V2Int.Zero;
    }
}
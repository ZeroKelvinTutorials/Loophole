using System.Text;
using core;


public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 2)
        {
            Console.WriteLine("Testing file!");
            Console.WriteLine("first argument should be turns, second should be file to test");
            //Evaluate given map
            string fileName = args[1];
            int minTurns = int.Parse(args[0]);
            string[] lines = File.ReadAllLines(fileName);
            Level level = LevelFactory.CreateLevel(lines);
            level.PrintAll(true);

            if (SolutionController.FindShortestPath(level, out GameState winState))
            {
                if (SolutionController.VerifySolution(level, minTurns))
                {
                    Console.WriteLine("Good level found !");
                    string[] levelLines = level.PrintAll(true);
                    File.WriteAllLines("GeneratedMaps/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", levelLines);
                    return;
                }
            }
            Console.WriteLine("Did not pass the test");
        }
        else
        {
            Console.WriteLine("Creating maps infinitely");
            Console.WriteLine("We need the first argument to be desired Holes used");
            string lastDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            int dateCounter = 0;

            while (true)
            {
                int minTurns = int.Parse(args[0]);
                Level level = LevelFactory.GenerateRandomLevel(7, 7, 2, 37, 10);
                if (SolutionController.VerifySolution(level, minTurns))
                {
                    Console.WriteLine("Good level found !" + " turns: " + minTurns);
                    string[] levelLines = level.PrintAll(true);
                    string dateStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (lastDate == dateStamp)
                    {
                        dateCounter++;
                    }
                    else
                    {
                        dateCounter = 0;
                    }

                    File.WriteAllLines("GeneratedMaps/turns" + minTurns + DateTime.Now.ToString("yyyyMMddHHmmss") + dateCounter + ".txt", levelLines);
                }
            }
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
}
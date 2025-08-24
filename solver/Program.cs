using System.Text;
using core;


public static class Program
{
    public static void Main(string[] args)
    {
        string file = args[0];
        string[] lines = File.ReadAllLines(file);

        Level level = LevelFactory.CreateLevel(lines);
        if (SolutionController.FindShortestPath(level, out GameState winState))
        {
            foreach (V2Int move in winState.moves)
            {
                Console.WriteLine(move.ToString());
            }
        }
    }
}
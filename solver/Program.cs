using System.Text;
using core;


public static class Program
{
    public static void Main()
    {
        string[] lines = File.ReadAllLines("Levels/Level2.txt");

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
using System;
using System.Collections.Generic;
using System.Text;

namespace core
{
    public class Level
    {
        public Board[] boards; //0 will be starting point (top)
        public int activeBoardIndex;
        public Player player;
        public event Action<V2Int> OnHoleEntered;
        public event Action<V2Int> OnWin;

        public Level(Board[] boards, int activeBoardIndex, Player player)
        {
            this.boards = new Board[boards.Length];


            for (int i = 0; i < boards.Length; i++)
            {
                this.boards[i] = (Board)boards[i].Clone();
            }

            this.activeBoardIndex = activeBoardIndex;
            this.player = (Player)player.Clone();
            this.OnWin = null;
            this.OnHoleEntered = null;
        }

        public void InvokeOnHoleEntered(V2Int holePosition)
        {
            OnHoleEntered?.Invoke(holePosition);
        }

        public void InvokeOnWin(V2Int goalPosition)
        {
            OnWin?.Invoke(goalPosition);
        }
        public Level Clone()
        {
            return new Level(this.boards, this.activeBoardIndex, this.player);
        }

        //TODO: Wall+Player, Box+Player, Wall+Box, Wall+Box+Player etc...
        public string[] PrintAll(bool toConsole = false)
        {
            List<string> lines = new List<string>();

            foreach (Board board in boards)
            {
                for (int y = 0; y < board.height; y++)
                {
                    StringBuilder line = new StringBuilder();

                    for (int x = 0; x < board.width; x++)
                    {

                        V2Int pos = new V2Int(x, y);
                        if (this.player.position == pos && boards[activeBoardIndex] == board)
                        {
                            line.Append("P");
                        }
                        else if (board.boxes.ContainsKey(pos))
                        {
                            line.Append("B");
                        }
                        else if (board.goal != null && board.goal.position == pos)
                        {
                            line.Append("G");
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
                    lines.Add(line.ToString());
                    if (toConsole)
                    {
                        Console.WriteLine(line.ToString());
                    }
                }
                lines.Add("");
                if (toConsole)
                {
                    Console.WriteLine();
                }
            }

            lines.RemoveAt(lines.Count - 1);

            return lines.ToArray();
        }
    }
}
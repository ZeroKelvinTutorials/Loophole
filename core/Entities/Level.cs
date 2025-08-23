using System;

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
    }
}
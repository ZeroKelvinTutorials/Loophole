using System;
using System.Collections.Generic;

namespace core
{
    public class Board : ICloneable
    {
        public int width;
        public int height;
        public Dictionary<V2Int, Tile> tiles = new Dictionary<V2Int, Tile>();

        //boxes and goals should ghmmm move to level? but no because they lose context of their level, we just need a way to smoothly transition boxes from one board to another...
        public Dictionary<V2Int, Box> boxes = new Dictionary<V2Int, Box>();
        public Goal goal = null;

        //Deep copy board
        public object Clone()
        {
            Board clone = new Board();

            clone.width = width;
            clone.height = height;

            clone.tiles = new Dictionary<V2Int, Tile>();
            foreach (KeyValuePair<V2Int, Tile> entry in tiles)
            {
                clone.tiles.Add(entry.Key, (Tile)entry.Value.Clone());
            }

            clone.boxes = new Dictionary<V2Int, Box>();
            foreach (KeyValuePair<V2Int, Box> entry in boxes)
            {
                clone.boxes.Add(entry.Key, (Box)entry.Value.Clone());
            }

            if (goal != null)
            {
                clone.goal = (Goal)goal.Clone();
            }

            return clone;
        }

        public Tile GetTile(V2Int position)
        {
            //Wrap on x and y


            if (!tiles.ContainsKey(position))
            {
                //Should not happen theoritcally due to the wrapping
                return null;
            }

            return tiles[position];
        }
    }
}
using System;

namespace core
{
    public class Tile : ICloneable
    {
        public TileType tileType;

        public Tile(TileType tileType)
        {
            this.tileType = tileType;
        }

        public object Clone()
        {
            Tile clone = new Tile(tileType);

            return clone;
        }
    }
}
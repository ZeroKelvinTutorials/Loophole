using System;

namespace core
{
    public class Goal : ICloneable
    {
        public V2Int position;

        public object Clone()
        {
            Goal clone = new Goal();
            clone.position = position;

            return clone;
        }
    }
}
namespace core
{
    public struct V2Int
    {
        public int x;
        public int y;

        public V2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static V2Int operator +(V2Int a, V2Int b)
        {
            return new V2Int(a.x + b.x, a.y + b.y);
        }

        public static V2Int operator -(V2Int a, V2Int b)
        {
            return new V2Int(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(V2Int a, V2Int b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(V2Int a, V2Int b)
        {
            return !(a == b);
        }

        public static V2Int Zero
        {
            get
            {
                return new V2Int(0, 0);
            }
        }
        public static V2Int Left
        {
            get
            {
                return new V2Int(-1, 0);
            }
        }
        public static V2Int Right
        {
            get
            {
                return new V2Int(1, 0);
            }
        }
        public static V2Int Up
        {
            get
            {
                return new V2Int(0, 1);
            }
        }
        public static V2Int Down
        {
            get
            {
                return new V2Int(0, -1);
            }
        }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        //should be a extension maybe
        public V2Int Loop(int maxX, int maxY)
        {
            if (x >= maxX)
            {
                x = 0;
            }
            if (x < 0)
            {
                x = maxX - 1;
            }
            if (y >= maxY)
            {
                y = 0;
            }
            if (y < 0)
            {
                y = maxY - 1;
            }
            return this;
        }

        public V2Int InvertYAxis()
        {
            return new V2Int(x, -y);
        }
    }
}
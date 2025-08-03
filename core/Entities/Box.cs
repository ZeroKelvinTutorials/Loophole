using System;

namespace core
{
    public class Box : ICloneable
    {
        public V2Int position;
        public event Action<V2Int> OnMove;
        public event Action<V2Int> OnFall;

        public object Clone()
        {
            Box clone = new Box();
            clone.position = position;
            clone.OnMove = null;
            clone.OnFall = null;

            return clone;
        }

        public void InvokeOnMove(V2Int targetPosition)
        {
            OnMove?.Invoke(targetPosition);
        }

        public void InvokeOnFall(V2Int targetPosition)
        {
            OnFall?.Invoke(targetPosition);
        }
    }
}
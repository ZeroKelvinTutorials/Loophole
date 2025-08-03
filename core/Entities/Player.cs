using System;

namespace core
{
    public class Player : ICloneable
    {
        public V2Int position;
        public V2Int direction;
        public event Action<V2Int> OnMove;
        public event Action OnStartMove;

        public object Clone()
        {
            Player clone = new Player();

            clone.position = position;
            clone.direction = direction;
            clone.OnMove = null;

            return clone;
        }

        public void InvokeOnMove(V2Int targetPosition)
        {
            OnMove?.Invoke(targetPosition);
        }

        public void InvokeOnStartMove()
        {
            OnStartMove?.Invoke();
        }
    }
}
using UnityEngine;
using core;
namespace view
{
    public class PlayerView : MonoBehaviour
    {
        public Player player;
        public void Subscribe()
        {
            player.OnMove += Move;
        }
        public void UnSubscribe()
        {
            player.OnMove -= Move;
        }
        public void Move(V2Int targetPosition)
        {
            transform.position = targetPosition.InvertYAxis().ToVector3();
        }
        public void OnDestroy()
        {
            Debug.Log("Unsubscribe player");
            UnSubscribe();
        }
    }
}
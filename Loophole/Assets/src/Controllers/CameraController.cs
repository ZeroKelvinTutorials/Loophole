using core;
using UnityEngine;

namespace view
{
    public static class CameraController
    {
        public static void Center(Board board)
        {
            float x = (float)(board.width - 1) / 2;
            float y = (float)(board.height - 1) / 2;

            Camera.main.transform.position = new Vector3(x, -y, -10);
        }
    }
}
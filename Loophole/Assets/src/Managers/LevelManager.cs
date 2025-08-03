using UnityEngine;

namespace view
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public GameObject groundPrefab;
        public GameObject holePrefab;
        public GameObject rockPrefab;
        public GameObject playerPrefab;
        public GameObject goalPrefab;
        public GameObject boxPrefab;

        public TextAsset[] levels;

        void Awake()
        {
            Instance = this;
        }


        public bool redraw = false;
        void Update()
        {
            if (redraw)
            {
                LevelController.RestartLevel();
                redraw = false;
            }
        }
    }
}
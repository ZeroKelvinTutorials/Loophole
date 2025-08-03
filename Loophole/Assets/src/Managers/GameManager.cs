using core;
using UnityEngine;
using view;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int currentLevelNumber;
    public static Level level;
    public static PlayerView playerView;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerInput.canTakeInput = true;
        GameController.OpenLevel(1);
    }

    void Update()
    {
        if (!PlayerInput.canTakeInput)
        {
            return;
        }
        if (PlayerInput.GetDirection(out Vector3 direction))
        {
            UndoManager.AddLevelHistory(GameManager.level);
            bool moved = PlayerController.TryMove(level.player, level, direction.ToV2Int().InvertYAxis());
            if (!moved)
            {
                UndoManager.levelHistory.RemoveAt(UndoManager.levelHistory.Count - 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            LevelController.Undo();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            LevelController.RestartLevel();
        }
    }
}

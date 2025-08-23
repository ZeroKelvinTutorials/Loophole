using System.Collections;
using System.Collections.Generic;
using core;
using UnityEngine;

namespace view
{
    public static class LevelController
    {
        public static void DrawLevel(Level level, bool transition = false)
        {
            level.OnHoleEntered += BoardTransition;
            level.OnWin += GameController.LevelWin;

            Board activeBoard = level.boards[level.activeBoardIndex];

            foreach (KeyValuePair<V2Int, Tile> entry in activeBoard.tiles)
            {
                GameObject prefab = null;

                switch (entry.Value.tileType)
                {
                    case TileType.Ground:
                        prefab = LevelManager.Instance.groundPrefab;
                        break;
                    case TileType.Rock:
                        prefab = LevelManager.Instance.rockPrefab;
                        break;
                    case TileType.Hole:
                        prefab = LevelManager.Instance.holePrefab;
                        break;
                    default:
                        Debug.LogError("Unassigned tile type " + entry.Value.tileType);
                        break;
                }

                GameObject.Instantiate(prefab, entry.Key.InvertYAxis().ToVector3(), Quaternion.identity, LevelManager.Instance.transform);
            }

            //Goal
            if (activeBoard.goal != null)
            {
                GoalView goalView = GameObject.Instantiate(LevelManager.Instance.goalPrefab, activeBoard.goal.position.InvertYAxis().ToVector3(), Quaternion.identity, LevelManager.Instance.transform).GetComponent<GoalView>();
            }

            //Player
            PlayerView playerView = GameObject.Instantiate(LevelManager.Instance.playerPrefab, level.player.position.InvertYAxis().ToVector3(), Quaternion.identity, LevelManager.Instance.transform).GetComponent<PlayerView>();
            GameManager.playerView = playerView;
            playerView.player = level.player;
            playerView.Subscribe();

            //Boxes
            foreach (Box box in activeBoard.boxes.Values)
            {
                BoxView boxView = GameObject.Instantiate(LevelManager.Instance.boxPrefab, box.position.InvertYAxis().ToVector3(), Quaternion.identity, LevelManager.Instance.transform).GetComponent<BoxView>();
                boxView.box = box;
                boxView.Subscribe();
            }

            CameraController.Center(activeBoard);
        }

        public static void BoardTransition(V2Int position, Board newBoard)
        {
            LevelManager.Instance.StartCoroutine(BoardTransitionCoroutine(position));
        }

        static IEnumerator BoardTransitionCoroutine(V2Int position)
        {
            GameManager.playerView.transform.position = position.InvertYAxis().ToVector3();
            PlayerInput.canTakeInput = false;

            WaitForFixedUpdate wait = new WaitForFixedUpdate();

            //Animate fall into hole
            float fallTime = .5f;
            float timer = 0;
            while (timer < fallTime)
            {
                timer += Time.fixedDeltaTime;
                if (timer > fallTime)
                {
                    timer = fallTime;
                }
                float progress = timer / fallTime;
                //TODO: smooth lerp ? 

                float scale = Mathf.Lerp(1, 0, progress);
                GameManager.playerView.transform.localScale = new Vector3(scale, scale, scale);

                yield return wait;
            }

            DestroyActiveLevelView();

            DrawLevel(GameManager.level);

            //Animate fall into map
            fallTime = .5f;
            timer = 0;
            while (timer < fallTime)
            {
                timer += Time.fixedDeltaTime;
                if (timer > fallTime)
                {
                    timer = fallTime;
                }
                float progress = timer / fallTime;
                //TODO: smooth lerp ? 

                float scale = Mathf.Lerp(5, 1, progress);
                GameManager.playerView.transform.localScale = new Vector3(scale, scale, scale);

                yield return wait;
            }

            PlayerInput.canTakeInput = true;
        }

        static void DestroyDrawnLevel()
        {

        }

        public static void DestroyActiveLevelView()
        {
            //Send maybe to a level view object that reflects level object
            GameManager.level.OnHoleEntered -= BoardTransition;

            foreach (Transform child in LevelManager.Instance.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void RestartLevel()
        {
            DestroyActiveLevelView();

            GameController.OpenLevel(GameManager.currentLevelNumber);
        }

        public static void Undo()
        {
            if (UndoManager.TryGetLastLevel(out Level lastLevel))
            {
                DestroyActiveLevelView();
                GameManager.level = lastLevel;
                DrawLevel(lastLevel);
            }
        }
    }
}
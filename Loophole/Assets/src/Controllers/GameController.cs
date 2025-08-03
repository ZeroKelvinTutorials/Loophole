using System;
using System.IO;
using core;
using UnityEngine;

namespace view
{
    public static class GameController
    {
        public static void OpenLevel(int levelNumber)
        {
            Debug.Log("Open level " + levelNumber);
            GameManager.currentLevelNumber = levelNumber;
            string[] lines = LevelManager.Instance.levels[levelNumber - 1].text.Split('\n');
            Debug.Log(lines.Length);
            Level level = LevelFactory.CreateLevel(lines);
            GameManager.level = level;

            UndoManager.InitializeLevelHistory();

            LevelController.DrawLevel(level);
        }


        public static void LevelWin(V2Int goalPosition)
        {
            PlayerInput.canTakeInput = false;
            //TODO: Turn on level won screen or whatever will happen...
            UIManager.Instance.levelWonUI.SetActive(true);
        }
    }
}
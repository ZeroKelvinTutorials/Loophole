using System.Collections.Generic;
using System.Linq;
using core;
using Unity.VisualScripting;

namespace view
{
    public static class UndoManager
    {
        public static List<Level> levelHistory = new List<Level>();

        public static void InitializeLevelHistory()
        {
            levelHistory = new List<Level>();
        }

        public static void AddLevelHistory(Level level)
        {
            levelHistory.Add(level.Clone());
        }

        public static bool TryGetLastLevel(out Level lastLevel)
        {
            lastLevel = null;
            if (levelHistory.Count == 0)
            {
                return false;
            }
            lastLevel = levelHistory[levelHistory.Count - 1];
            levelHistory.RemoveAt(levelHistory.Count - 1);
            return true;
        }
    }
}
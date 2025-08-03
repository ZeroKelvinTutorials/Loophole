using UnityEngine;
using view;
public class LevelWonButton : MonoBehaviour
{
    public void Press()
    {
        LevelController.DestroyActiveLevelView();
        PlayerInput.canTakeInput = true;
        GameController.OpenLevel(GameManager.currentLevelNumber + 1);
        this.gameObject.SetActive(false);
    }
}
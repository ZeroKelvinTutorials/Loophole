using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using view;
public class LevelWonButton : MonoBehaviour
{
    public Text text;
    public void Press()
    {
        LevelController.DestroyActiveLevelView();
        PlayerInput.canTakeInput = true;
        if (GameManager.currentLevelNumber == LevelManager.Instance.levels.Length)
        {
            GameController.OpenLevel(1);
        }
        else
        {
            GameController.OpenLevel(GameManager.currentLevelNumber + 1);
        }
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (GameManager.currentLevelNumber == LevelManager.Instance.levels.Length)
        {
            text.text = "You made it to the end, Thankyou!";
        }
        else
        {
            text.text = "Next Level";
        }
    }
}
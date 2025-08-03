using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject levelWonUI;

    void Awake()
    {
        Instance = this;
    }
}
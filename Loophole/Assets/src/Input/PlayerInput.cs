using UnityEngine;

public static class PlayerInput
{
    public static bool canTakeInput;

    public static bool GetDirection(out Vector3 result)
    {
        result = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            result = Vector3.left;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            result = Vector3.up;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            result = Vector3.down;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            result = Vector3.right;
            return true;
        }
        else
        {
            return false;
        }
    }
}
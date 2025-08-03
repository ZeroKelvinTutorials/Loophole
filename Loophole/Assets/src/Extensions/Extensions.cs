using UnityEngine;
using core;
public static class Extensions
{
    public static Vector3 ToVector3(this V2Int v2Int)
    {
        return new Vector3(v2Int.x, v2Int.y, 0);
    }

    public static V2Int ToV2Int(this Vector3 vector3)
    {
        return new V2Int((int)vector3.x, (int)vector3.y);
    }
}
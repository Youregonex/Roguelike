using UnityEngine;

public static class Utilities
{
    public static Vector2Int GetMouseSnapedPosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Vector2Int.RoundToInt(mousePosition);
    }
}

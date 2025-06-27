using UnityEngine;

public static class Utilities
{
    public static Vector2Int GetMouseSnapedPosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Vector2Int.RoundToInt(mousePosition);
    }

    public static T GetRootComponent<T>(Transform transform) where T : Component
    {
        T component;
        Transform current = transform;
        while (current.parent != null)
        {
            if(current.TryGetComponent(out component))
                return component;

            current = current.parent;
        }

        if (current.TryGetComponent(out component))
            return component;
        else
        {
            Debug.LogError($"Couldn't find {typeof(T)} component!");
            return null;
        }
    }

    public static Vector2 GetDirectionVectorNormalized(Vector2 self, Vector2 target, bool oppositeDirection = false)
    {
        Vector2 resultDirection = (target - self).normalized;
        return oppositeDirection ? -resultDirection : resultDirection;
    }
}

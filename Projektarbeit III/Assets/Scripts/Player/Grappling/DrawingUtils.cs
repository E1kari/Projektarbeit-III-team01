using UnityEngine;

public static class DrawingUtils
{
    // Indicator
    public static void DrawCircle(LineRenderer lineRenderer, Vector2 position, float radius, int segments)
    {
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }

    // LineRenderer
}
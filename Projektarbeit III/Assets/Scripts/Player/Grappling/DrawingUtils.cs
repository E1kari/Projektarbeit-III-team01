using UnityEngine;

public static class DrawingUtils
{
    // Indicator
    public static void DrawCircle(LineRenderer Indicator, Vector2 position, float radius, int segments)
    {
        Indicator.positionCount = segments + 1;
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            Indicator.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }

    // LineRenderer
    public static LineRenderer InitializeLineRenderer(GameObject gameObject, float startWidth, float endWidth)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.positionCount = 0;
        return lineRenderer;
    }

    public static void UpdateLineRenderer(LineRenderer lineRenderer, Vector2 startPosition, Vector2 endPosition)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    public static void HideLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }
}
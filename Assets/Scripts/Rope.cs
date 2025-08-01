using UnityEngine;

public class Rope : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private bool CanAppend(Vector2 position)
    {
        if (lineRenderer.positionCount == 0)
        {
            return true;
        }

        return Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), position) > 0.1f; 
    }

    public void SetPosition(Vector2 position)
    {
        if (!CanAppend(position))
        {
            return;
        }

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }
}

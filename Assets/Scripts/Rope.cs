using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private int maxNumPoints = 80;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;

    private List<Vector2> points = new List<Vector2>();

    private void Start()
    {
        edgeCollider.transform.position -= transform.position;
    }

    private bool CanAppend(Vector2 position)
    {
        if (lineRenderer.positionCount == 0)
        {
            return true;
        }

        if (lineRenderer.positionCount > maxNumPoints)
        {
            return false;
        }

        return Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), position) > 0.1f; 
    }

    public void SetPosition(Vector2 position)
    {
        if (!CanAppend(position))
        {
            return;
        }

        points.Add(position);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
        edgeCollider.points = points.ToArray();
    }

    //private void OnDestroy()
    //{
    //    Debug.Log(lineRenderer.positionCount);
    //}
}

using System;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    // Must have particle system attached
    [SerializeField] private GameObject capturePointParticles;
    private int numTriggers = default;
    private ParticleSystem successParticles;
    private CapturedEnemyContainer enemyContainer = null;


    private void Awake()
    {
        successParticles = capturePointParticles.GetComponent<ParticleSystem>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lasso"))
        {
            numTriggers++;
            CheckForFull();
        }
    }

    private void CheckForFull()
    {
        if (numTriggers < 2)
        {
            return;
        }

        numTriggers = 0;
        GameObject[] ropePoints = GameObject.FindGameObjectsWithTag("Rope");

        LineRenderer lineRenderer = ropePoints[0].GetComponent<LineRenderer>();
        if (lineRenderer.positionCount < 3)
        {
            Debug.Log("Capture Circle does not have enough points");
            return;
        }

        CircleCollider2D circleCollider = CreateCircleCollider(lineRenderer);

        if(CheckIfEnemiesAreInCaptureCircle(circleCollider))
        {
            GameObject particles = Instantiate(capturePointParticles, transform.position, Quaternion.identity);
            Destroy(particles, successParticles.totalTime + 3);
        }

        Destroy(circleCollider.gameObject);

        Array.ForEach(ropePoints, ropePoint => { Destroy(ropePoint); });
    }

    private bool CheckIfEnemiesAreInCaptureCircle(CircleCollider2D captureCircle)
    {
        bool isEnemyInCircle = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider == null) continue;

            if (DoCollidersFullyOverlap(enemyCollider, captureCircle))
            {
                isEnemyInCircle = true;
                Debug.Log($"Enemy '{enemy.name}' has been hit!");
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                bool isDead = enemyController.Damage(50);

                if(isDead)
                {
                    enemyController.isCaptured = true;
                    CaptureEnemy(enemy);
                }
            }
        }

        return isEnemyInCircle;
    }

    // GameObject passed as param MUST have EnemyController attached to it
    private void CaptureEnemy(GameObject enemy)
    {
        if (enemyContainer == null)
        {
            GameObject[] objectsTaggedAsPlayer = GameObject.FindGameObjectsWithTag("Player");
            enemyContainer = objectsTaggedAsPlayer[0].GetComponent<CapturedEnemyContainer>();
        }

        enemyContainer.AddEnemy(enemy);
    }

    private CircleCollider2D CreateCircleCollider(LineRenderer lineRenderer)
    {
        int positionCount = lineRenderer.positionCount;
        Vector3[] positions = new Vector3[positionCount];
        lineRenderer.GetPositions(positions);

        // Ensure all points are in world space for accurate calculation
        Vector3[] worldPositions = new Vector3[positionCount];
        if (lineRenderer.useWorldSpace)
        {
            // If the line renderer is already using world space, just copy the array
            Array.Copy(positions, worldPositions, positionCount);
        }
        else
        {
            // If not, convert each local point to its world space equivalent
            for (int i = 0; i < positionCount; i++)
            {
                worldPositions[i] = lineRenderer.transform.TransformPoint(positions[i]);
            }
        }

        // Calculate the center in world space
        Vector3 center = Vector3.zero;
        foreach (Vector3 pos in worldPositions)
        {
            center += pos;
        }
        center /= positionCount;

        // Calculate the radius based on the farthest point from the world space center
        float radius = 0f;
        foreach (Vector3 pos in worldPositions)
        {
            float dist = Vector3.Distance(pos, center);
            if (dist > radius)
            {
                radius = dist;
            }
        }

        // Create a new, unparented GameObject for the collider
        GameObject colliderObj = new GameObject("CaptureCircleCollider");
        colliderObj.transform.position = center; // Set its position directly in world space

        CircleCollider2D circleCollider = colliderObj.AddComponent<CircleCollider2D>();
        circleCollider.radius = radius;
        circleCollider.isTrigger = true;

        Debug.Log($"Collider created at world position {center} with radius {radius}.");
        return circleCollider;
    }


    private bool DoCollidersFullyOverlap(Collider2D enemyCollider, CircleCollider2D lassoCollider)
    {
        Vector2 circleCenter = lassoCollider.transform.position;
        float circleRadius = lassoCollider.radius;
        Bounds innerBounds = enemyCollider.bounds;
        Vector2 farthestPoint = innerBounds.ClosestPoint(circleCenter + (circleCenter - (Vector2)innerBounds.center).normalized * circleRadius * 2);
        return Vector2.Distance(circleCenter, farthestPoint) < circleRadius;
    }
}

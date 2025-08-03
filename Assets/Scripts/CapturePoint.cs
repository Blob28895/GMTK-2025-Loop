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
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);


        Vector3 center = Vector3.zero;
        foreach (Vector3 pos in positions)
        {
            center += pos;
        }
        center /= lineRenderer.positionCount;


        float radiusSq = 0f;
        foreach (Vector3 pos in positions)
        {
            float distSq = (pos - center).sqrMagnitude;
            if (distSq > radiusSq)
            {
                radiusSq = distSq;
            }
        }
        float radius = Mathf.Sqrt(radiusSq);


        GameObject colliderObj = new GameObject("CircleCollider");

        colliderObj.transform.SetParent(lineRenderer.transform);
        colliderObj.transform.localPosition = center;

        CircleCollider2D circleCollider = colliderObj.AddComponent<CircleCollider2D>();
        circleCollider.radius = radius;

        circleCollider.isTrigger = true;

        Debug.Log($"Collider created at {center} with radius {radius}.");
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

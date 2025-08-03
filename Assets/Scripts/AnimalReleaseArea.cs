using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalReleaseArea : MonoBehaviour
{
    [SerializeField] private Transform[] releasePoints;

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            CapturedEnemyContainer enemyContainer = other.gameObject.GetComponent<CapturedEnemyContainer>();
            releaseAnimal(enemyContainer);
        }
    }

    private void releaseAnimal(CapturedEnemyContainer enemyContainer)
    {
        List<EnemyController> enemies = enemyContainer.getCapturedEnemies();
        enemyContainer.resetCapturedEnemies();

        foreach (EnemyController enemy in enemies)
        {
            Transform releasePoint = releasePoints[UnityEngine.Random.Range(0, releasePoints.Length)];

            enemy._movementState = EnemyController.MovementState.wandering;
            enemy.gameObject.transform.SetParent(null, false);
            enemy.gameObject.transform.position = releasePoint.position;
        }
    }
}

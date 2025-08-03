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

        //Debug.Log("Releasing " + enemies.Count + " animals.");

        foreach (EnemyController enemy in enemies)
        {
            Transform releasePoint = releasePoints[UnityEngine.Random.Range(0, releasePoints.Length)];
            enemy.makeCaptive(releasePoint);
        }
    }
}

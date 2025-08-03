using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturedEnemyContainer : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    private List<EnemyController> _capturedEnemies = new List<EnemyController>();

    public void AddEnemy(GameObject enemy)
    {
        _capturedEnemies.Add(enemy.GetComponent<EnemyController>());
        enemy.transform.SetParent(_container.transform, false);
        enemy.transform.transform.localPosition = Vector3.zero;
    }

    public void SetMovementState(EnemyController.MovementState state)
    {
        foreach (EnemyController enemy in _capturedEnemies)
        {
            enemy._movementState = state;
        }
    }

    public List<EnemyController> getCapturedEnemies()
    {
        return _capturedEnemies;
    }

    public void resetCapturedEnemies()
    {
        _capturedEnemies = new List<EnemyController>();
    }
}

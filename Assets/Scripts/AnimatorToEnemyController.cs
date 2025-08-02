using UnityEngine;

public class AnimatorToEnemyController : MonoBehaviour
{


    /// <summary>
    /// This script is literally just so that the animator can communicate with its parent object 
    /// </summary>
    /// 
    private EnemyController _enemyController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
    }

    //called as an animation event
    void controllerAttack()
    {
        _enemyController.attack();
    }

    void controllerStopAttack()
    {
        _enemyController.stopAttack();
    }
}

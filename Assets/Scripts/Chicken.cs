using UnityEngine;

public class Chicken : MonoBehaviour
{
    private Animator _animator;
    private EnemyController _enemyController;

    [SerializeField]
    private float wanderAnimSpeed = 1f;
    [SerializeField]
    private float chaseAnimSpeed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("moving", true);
        _enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController._movementState == EnemyController.MovementState.wandering)
        {
            _animator.SetFloat("walkAnimSpeed", wanderAnimSpeed);
        }
		else if (_enemyController._movementState == EnemyController.MovementState.chasing)
		{
			_animator.SetFloat("walkAnimSpeed", chaseAnimSpeed);
		}

	}
}

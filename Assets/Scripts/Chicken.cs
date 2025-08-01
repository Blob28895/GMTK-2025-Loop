using System;
using System.Numerics;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private Animator _animator;
    private EnemyController _enemyController;
	private Transform _targetPosition;
    private float cooldownTimer = 0f;

	[SerializeField]
    private float wanderAnimSpeed = 1f;
    [SerializeField]
    private float chaseAnimSpeed = 3f;
    [SerializeField]
    private float attackRange = 1.5f;
    [SerializeField]
    private float attackCooldown = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("moving", true);
        _enemyController = GetComponent<EnemyController>();
		_targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
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

		if(UnityEngine.Vector3.Distance(_targetPosition.position, transform.position) < attackRange && cooldownTimer <= 0)
        {
            _animator.SetTrigger("Attack");
            cooldownTimer = attackCooldown;
        }

	}

	private void FixedUpdate()
	{
		if( cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
	public enum MovementState
	{
		wandering,
		chasing
	}

	private Transform _targetPosition;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
	private Vector2 _startingPosition;
    private Vector2 _delta;
	private Animator _animator;
	private float cooldownTimer = 0f;
	public MovementState _movementState { get; set; }

	[Header("Damage Settings")]
    [SerializeField] private float _damageFrequency = 1f;
    [SerializeField] private int _damage = 1;
    //[SerializeField] private GameObject _damageEffect = default;

    [Header("Layer Settings")]
    [SerializeField] private Collider2D _collider;

    [Header("Movement Settings")]
    [SerializeField] private float chaseSpeed = 1f;
	[SerializeField] private float chaseAnimSpeed = 3f;
	[SerializeField] private float wanderSpeed = 1f;
	[SerializeField] private float wanderAnimSpeed = 1f;
	[Tooltip("Amount of time that the wander AI waits to change directions")]
    [SerializeField] private float wanderDirectionChangeCooldown = 1f;
    [Tooltip("Maximum distance entity will wander from its start point.")]
    [SerializeField] private float wanderDistance = 5f;

    [Header("Temperment Settings")]
    [Tooltip("Distance at which an enemy will see the player and decide to start chasing them. The larger the number the larger the range.")]
    [SerializeField] private float aggroRange = 10f;
    [Tooltip("Distance at which an enemy will stop chasing the player, should be longer than the aggro range.")]
    [SerializeField] private float loseAggroRange = 12f;
	[SerializeField] private float attackRange = 1.5f;
	[SerializeField] private float attackCooldown = 5f;

	//[Header("Audio")]
	// [SerializeField] private AudioSource _passiveNoise;
	//[Tooltip("Object that will spawn to play the enemy death noise when an enemy dies. Since it cant be playing a sound while also destroying itself")]
	//[SerializeField] private GameObject _deathSoundObject;


	void Start()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _delta = transform.position;
        _startingPosition = transform.position;
		_animator = GetComponent<Animator>();
		_animator.SetBool("moving", true);
		// _passiveNoise = GetComponent<AudioSource>();
	}

    private void FixedUpdate()
    {

		if (cooldownTimer > 0) { cooldownTimer -= Time.deltaTime; }
		
        if ( //If we are wandering and the player is in aggro range, or if we are chasing and the player isnt out of aggro range
            _movementState == MovementState.wandering && Vector3.Distance(_targetPosition.position, transform.position) <= aggroRange ||
			_movementState == MovementState.chasing && Vector3.Distance(_targetPosition.position, transform.position) <= loseAggroRange) 
        {
			Vector3 direction = (_targetPosition.position - transform.position).normalized;
            _movementState = MovementState.chasing;
            move(new Vector2(direction.x, direction.y), chaseSpeed);
		}
        else
        {
            wander();
        }
    }

    private void wander()
    {
		_movementState = MovementState.wandering;
		wanderDirectionChangeCooldown -= Time.deltaTime;

		if (wanderDirectionChangeCooldown <= 0)
		{
            Vector3 targetPosition = new Vector3(
                _startingPosition.x + UnityEngine.Random.Range(-1 * wanderDistance, wanderDistance),
                _startingPosition.y + UnityEngine.Random.Range(-1 * wanderDistance, wanderDistance), 0);

            _delta = (targetPosition - transform.position).normalized;

			wanderDirectionChangeCooldown = UnityEngine.Random.Range(1f, 5f);
		}
        move(new Vector2(_delta.x, _delta.y), wanderSpeed);
	}

	private void move(Vector2 endposition, float speed)
	{

        _rb.MovePosition(_rb.position + endposition * speed * Time.deltaTime);
		if (endposition.x > 0)
		{
            _spriteRenderer.flipX = false;
		}
        else
        {
            _spriteRenderer.flipX = true;
        }
	}


	private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AttackPlayer(other, _damage));
        }
    }

    private IEnumerator AttackPlayer(Collider2D playerCollider, int damage)
    {
        //_damageEffect.SetActive(true);

        HealthSO playerHealth = playerCollider.GetComponent<PlayerController>().health;
        playerHealth.Damage(_damage);
        // _passiveNoise.Play();

        yield return new WaitForSeconds(_damageFrequency);
        Debug.Log("Attacking player");
        //_damageEffect.SetActive(false);
    }

	public void Update()
	{
		if (_movementState == MovementState.wandering)
		{
			_animator.SetFloat("walkAnimSpeed", wanderAnimSpeed);
		}
		else if (_movementState == MovementState.chasing)
		{
			_animator.SetFloat("walkAnimSpeed", chaseAnimSpeed);
		}

        Debug.Log(gameObject.name + " : " + cooldownTimer);
		if (Vector3.Distance(_targetPosition.position, transform.position) < attackRange && cooldownTimer <= 0)
		{
			_animator.SetTrigger("Attack");
			cooldownTimer = attackCooldown;
		}
	}


	public void Die()
    {
        // Instantiate(_deathSoundObject);
        Destroy(gameObject);
    }
}

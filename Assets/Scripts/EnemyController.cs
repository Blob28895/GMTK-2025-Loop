using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
    private Transform _targetPosition;
    private Rigidbody2D _rb;
	private Vector2 _startingPosition;
    private Vector2 _delta;

	[Header("Damage Settings")]
    [SerializeField] private float _damageFrequency = 1f;
    [SerializeField] private int _damage = 1;
    //[SerializeField] private GameObject _damageEffect = default;

    [Header("Layer Settings")]
    [SerializeField] private Collider2D _collider;

    [Header("Movement Settings")]
    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float wanderSpeed = 1f;
    [Tooltip("Amount of time that the wander AI waits to change directions")]
    [SerializeField] private float wanderDirectionChangeCooldown = 1f;
    [Tooltip("Maximum distance entity will wander from its start point.")]
    [SerializeField] private float wanderDistance = 5f;

    [Header("Temperment Settings")]
    [Tooltip("Distance at which an enemy will see the player and decide to start chasing them. The larger the number the larger the range.")]
    [SerializeField] private float aggroRange = 10f;

    //[Header("Audio")]
    // [SerializeField] private AudioSource _passiveNoise;
    //[Tooltip("Object that will spawn to play the enemy death noise when an enemy dies. Since it cant be playing a sound while also destroying itself")]
    //[SerializeField] private GameObject _deathSoundObject;


    void Start()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _delta = transform.position;
        _startingPosition = transform.position;
        // _passiveNoise = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(_targetPosition.position, transform.position) <= aggroRange)
        {
			Vector3 direction = (_targetPosition.position - transform.position).normalized;//.normalized;
			chase(direction);
		}
        else
        {
            wander();
        }
        

    }

    private void chase(Vector3 dir)
    {
        _rb.MovePosition(_rb.position + new Vector2(dir.x, dir.y) * chaseSpeed * Time.deltaTime);
    }

    private void wander()
    {

		wanderDirectionChangeCooldown -= Time.deltaTime;

		if (wanderDirectionChangeCooldown <= 0)
		{
            Vector3 targetPosition = new Vector3(
                _startingPosition.x + UnityEngine.Random.Range(-1 * wanderDistance, wanderDistance),
                _startingPosition.y + UnityEngine.Random.Range(-1 * wanderDistance, wanderDistance), 0);

            _delta = (targetPosition - transform.position).normalized;

			wanderDirectionChangeCooldown = UnityEngine.Random.Range(1f, 5f);
		}
		_rb.MovePosition(_rb.position + new Vector2(_delta.x, _delta.y) * wanderSpeed * Time.deltaTime);
	}


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Colliding with something");
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

    public void Die()
    {
        // Instantiate(_deathSoundObject);
        Destroy(gameObject);
    }
}

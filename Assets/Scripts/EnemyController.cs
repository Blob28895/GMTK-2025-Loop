using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
    private Transform _targetPosition;
    private Animator _animator;
    private Rigidbody2D _rb;

    [Header("Damage Settings")]
    [SerializeField] private float _damageFrequency = 1f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private GameObject _damageEffect = default;

    [Header("Layer Settings")]

    [Tooltip("This is all layers that the mouse will define as the ground")]
    [SerializeField] private LayerMask ground;

    [SerializeField] private Collider2D _walkingCollider;

    [Header("Speeds")]
    [SerializeField] private float walkingSpeed = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource _passiveNoise;
    [Tooltip("Object that will spawn to play the enemy death noise when an enemy dies. Since it cant be playing a sound while also destroying itself")]
    [SerializeField] private GameObject _deathSoundObject;


    void Start()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _passiveNoise = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (_targetPosition.position - transform.position);//.normalized;
        walk(direction);
    }

    private void walk(Vector3 dir)
    {
        if (Math.Abs(dir.x) < 0.05f) { return; }
        if (dir.x < 0f)
        {
            dir.x = -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            dir.x = 1;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        _rb.MovePosition(_rb.position + new Vector2(dir.x, 0) * walkingSpeed * Time.deltaTime);
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
        _damageEffect.SetActive(true);

        HealthSO playerHealth = playerCollider.GetComponent<PlayerController>().health;
        playerHealth.Damage(_damage);
        _passiveNoise.Play();

        yield return new WaitForSeconds(_damageFrequency);

        _damageEffect.SetActive(false);
        StartCoroutine(playerHealth.AttemptToRegen());
    }

    public void Die()
    {
        Instantiate(_deathSoundObject);
        Destroy(gameObject);
    }
}

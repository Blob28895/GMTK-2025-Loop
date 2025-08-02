using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public HealthSO health;
	[SerializeField] private InputReaderSO _inputReader = default;
	private CapturedEnemyContainer _capturedEnemyContainer;
	private SpriteRenderer _spriteRenderer = default;
	private Animator _animator;

	public int speed = 5;
	private bool canMove = true;

	// variables for player movement
	private Rigidbody2D rb;
	private ContactFilter2D movementFilter;
	private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
	private Vector2 lastDirectionFacing = Vector2.zero;


	private Vector2 _inputVector;
	//private AudioManager audioManager;


	void Awake()	
	{
		_capturedEnemyContainer = GetComponent<CapturedEnemyContainer>();
		rb = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_inputReader.EnableGameplayInput();
		_animator = GetComponentInChildren<Animator>();
		//audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

		//nextLevelName = getNextLevel(SceneManager.GetActiveScene().name);
	}
	private void Update()
	{
	}

	private void FixedUpdate()
	{
		if (canMove)
		{
			Move();
		}
	}

	private void OnRun(Vector2 movementInput)
	{
		if (movementInput.x > 0) { _spriteRenderer.flipX = true; }
		else if (movementInput.x < 0) { _spriteRenderer.flipX = false; }

		_inputVector = movementInput;
	}

	private void OnEnable()
	{
		_inputReader.RunEvent += OnRun;
	}

	private void OnDisable()
	{
		_inputReader.RunEvent -= OnRun;
	}


	private void Move()
	{
		float horizontalInput = _inputVector.x;
		float verticalInput = _inputVector.y;

		Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;
		if(movement == new Vector2(0, 0))
		{
			_animator.SetBool("moving", false);
		}
		else
		{
			_animator.SetBool("moving", true);
		}

		bool success = movePlayer(movement);
		if (!success)
		{
			success = movePlayer(new Vector2(movement.x, 0));
			if (!success)
			{
				movePlayer(new Vector2(0, movement.y));
			}
		}

		// make player face the last direction they were facing
		if (movement == Vector2.zero) { movement = lastDirectionFacing; }
		else { lastDirectionFacing = movement; }

	}

	private bool movePlayer(Vector2 vec)
	{
		int count = rb.Cast(
		vec, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
		movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
		castCollisions, // List of collisions to store the found collisions into after the Cast is finished
		speed * Time.fixedDeltaTime); // The amount to cast equal to the movement

		if (count == 0)
		{
			rb.MovePosition(rb.position + vec * speed * Time.fixedDeltaTime);
			_capturedEnemyContainer.SetMovementState(EnemyController.MovementState.wandering);
			return true;
		}

		_capturedEnemyContainer.SetMovementState(EnemyController.MovementState.idle);
		return false;
	}

	public void setCanMove(bool b)
	{
		canMove = b;
	}


}

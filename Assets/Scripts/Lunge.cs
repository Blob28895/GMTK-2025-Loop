using UnityEngine;
using static EnemyController;

public class Lunge : MonoBehaviour, Attacker
{
    [SerializeField] private float lungeSpeed = 10f;
    [SerializeField] private float lungeDamage = 10f;

    public bool attacking = false;
    private Transform _targetPosition;
    private Vector2 _attackDirection = new Vector2();
    private Rigidbody2D _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    //A lunge towards the player
    void Attacker.Attack(Transform targetPosition)
    {
        attacking = true;
        _targetPosition = targetPosition;
        Debug.Log("The attack link worked and is calling from the chicken script");
		_attackDirection = (_targetPosition.position - transform.position).normalized;
	}

	private void FixedUpdate()
	{
		if(attacking)
        {
            Debug.Log(_rb.position + " : " + _attackDirection);
			_rb.MovePosition(_rb.position + new Vector2(_attackDirection.x, _attackDirection.y) * lungeSpeed * Time.deltaTime);
		}
	}

    void Attacker.stopAttack()
    {
        attacking = false;
    }

}

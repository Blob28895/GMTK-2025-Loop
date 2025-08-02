using UnityEngine;
using static EnemyController;

public class Chicken : MonoBehaviour, Attacker
{
    [SerializeField] private float lungeDistance = 1f;
    [SerializeField] private float lungeSpeed = 10f;
    [SerializeField] private float lungeDamage = 10f;

    public bool attacking = false;
    private Transform _targetPosition;
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
    }

	private void FixedUpdate()
	{
		if(attacking)
        {
			Vector3 direction = (_targetPosition.position - transform.position).normalized;
			_rb.MovePosition(_rb.position + new Vector2(direction.x, direction.y) * lungeSpeed * Time.deltaTime);
		}
	}

    void Attacker.stopAttack()
    {
        attacking = false;
    }

}

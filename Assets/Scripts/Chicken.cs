using UnityEngine;
using static EnemyController;

public class Chicken : MonoBehaviour, Attacker
{
    [SerializeField] private float peckDistance = 1f;
    [SerializeField] private float peckSpeed = 10f;
    [SerializeField] private float peckDamage = 10f;

    public bool attacking = false;
    private Transform _targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //A peck, this code just lunges towards the player
    void Attacker.Attack(Transform targetPosition)
    {
        Debug.Log("The attack link worked and is calling from the chicken script");
    }

	private void FixedUpdate()
	{
		if(attacking)
        {

        }
	}


}

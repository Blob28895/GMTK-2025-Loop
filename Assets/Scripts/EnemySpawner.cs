using UnityEngine;


public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyType;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private float spawnTimer = 60f;
    [SerializeField] private int maxSpawns = 3;

    private int spawnCount;
    private float spawnCountdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < maxSpawns; ++i)
        {
            SpawnEnemy();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount >= maxSpawns)
        {
            spawnCountdown = spawnTimer;
        }
        if(transform.childCount < maxSpawns && spawnTimer <= 0)
        {
            SpawnEnemy();
        }
    }

	private void FixedUpdate()
	{
		if(spawnTimer > 0 && transform.childCount < maxSpawns)
        {
            spawnCountdown -= Time.deltaTime;
        }
	}

	private void SpawnEnemy()
    {
        Instantiate(enemyType, 
            new Vector3(Random.Range(-1 * spawnRadius, spawnRadius) + transform.position.x,
						Random.Range(-1 * spawnRadius, spawnRadius) + transform.position.y, 0),
                        new Quaternion(0f, 0f, 0f, 0f),
                        transform);
    }
}

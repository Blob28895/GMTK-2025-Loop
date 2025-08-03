using UnityEngine;

public class Producer : MonoBehaviour
{

    public enum Product
    {
        feathers,
        milk,
        horns,
        hair,
        wool,
        tallow
    }

    [SerializeField] public Product product;
    [Tooltip("How many seconds it will take for this producer to produce one of its material")]
    [SerializeField] private float generationRate = 4f;

    private float generationTimer;
    private bool captured = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generationTimer = generationRate;
    }

	private void FixedUpdate()
	{
		generationTimer -= Time.deltaTime;
        if(generationTimer <= 0f)
        {
            //generate a material
            StaticVariables.addMaterial(product);
            generationTimer = generationRate;
        }
	}

	public void setCaptured(bool b)
    {
        captured = b;
    }
}

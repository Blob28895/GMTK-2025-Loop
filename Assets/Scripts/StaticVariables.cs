using UnityEngine;

public class StaticVariables : MonoBehaviour
{

	//materials
	static int feathers = 0;
	static int hair = 0;
	static int horns = 0;
	static int tallow = 0;
	static int wool = 0;
	static int milk = 0;

	static public void addMaterial(Producer.Product p)
	{
		switch(p)
		{
			case Producer.Product.feathers:
				feathers++;
				break;
			case Producer.Product.hair:
				hair++;
				break;
			case Producer.Product.horns:
				horns++;
				break;
			case Producer.Product.tallow:
				tallow++;
				break;
			case Producer.Product.wool:
				wool++;
				break;
			case Producer.Product.milk:
				milk++;
				break;
			default:
				Debug.Log("INVALID PRODUCT TYPE");
				break;
		}
	}
}

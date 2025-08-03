using UnityEngine;

public class StaticVariables : MonoBehaviour
{
	public static int upgradeCost = 25;

	//materials
	static int feathers = 10;
	static int hair = 10;
	static int horns = 10;
	static int tallow = 10;
	static int wool = 10;
	static int milk = 10;

	//upgrade levels
	static int speedUpgradeLevel = 0;
	static int strengthUpgradeLevel = 0;
	static int healthUpgradeLevel = 0;
	static int lengthUpgradeLevel = 0;

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
	static public int getMaterialAmount(Producer.Product p)
	{
		switch(p)
		{
			case Producer.Product.feathers:
				return feathers;
			case Producer.Product.hair:
				return hair;
			case Producer.Product.horns:
				return horns;
			case Producer.Product.tallow:
				return tallow;
			case Producer.Product.wool:
				return wool;
			case Producer.Product.milk:
				return milk;
			default:
				Debug.Log("INVALID PRODUCT TYPE");
				return -1;
		}
	}

	static public void spendMaterials(Producer.Product p, int amount)
	{
		switch (p)
		{
			case Producer.Product.feathers:
				feathers -= amount;
				break;
			case Producer.Product.hair:
				hair -= amount;
				break;
			case Producer.Product.horns:
				horns -= amount;
				break;
			case Producer.Product.tallow:
				tallow -= amount;
				break;
			case Producer.Product.wool:
				wool -= amount;
				break;
			case Producer.Product.milk:
				milk -= amount;
				break;
			default:
				Debug.Log("INVALID PRODUCT TYPE");
				break;
		}
	}
	static public void increaseUpgradeLevel(upgradeManager.Upgrade u)
	{
		switch(u){
			case upgradeManager.Upgrade.speed:
				speedUpgradeLevel++;
				break;
			case upgradeManager.Upgrade.health:
				healthUpgradeLevel++;
				break;
			case upgradeManager.Upgrade.length:
				lengthUpgradeLevel++;
				break;
			case upgradeManager.Upgrade.strength:
				strengthUpgradeLevel++;
				break;
			default:
				Debug.Log("INVALID UPGRADE TYPE");
				break;
		}
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class upgradeManager : MonoBehaviour
{
    [Serializable]
    public enum Upgrade
    {
        health,
        speed,
        strength,
        length
    }

    private Dictionary<int, Upgrade> upgradeDict= new Dictionary<int, Upgrade>() {
        {1, Upgrade.health },
        {2, Upgrade.speed },
        {3, Upgrade.strength },
        {4, Upgrade.length }
    };

    private Dictionary<int, Tuple<Producer.Product, Producer.Product>> materialTypesDict =
        new Dictionary<int, Tuple<Producer.Product, Producer.Product>>()
        {
            { 1, new Tuple<Producer.Product, Producer.Product>(Producer.Product.milk, Producer.Product.tallow)  },
            { 2, new Tuple<Producer.Product, Producer.Product>(Producer.Product.feathers, Producer.Product.wool)  },
            { 3, new Tuple<Producer.Product, Producer.Product>(Producer.Product.hair, Producer.Product.horns)  },
            { 4, new Tuple<Producer.Product, Producer.Product>(Producer.Product.hair, Producer.Product.tallow)  },
        };


	public void upgradeStat(int i)
    {
        if (StaticVariables.upgradeCost > StaticVariables.getMaterialAmount(materialTypesDict[i].Item1) ||
           StaticVariables.upgradeCost > StaticVariables.getMaterialAmount(materialTypesDict[i].Item2))
        {
            return;
        }
        StaticVariables.spendMaterials(materialTypesDict[i].Item1, StaticVariables.upgradeCost);
        StaticVariables.spendMaterials(materialTypesDict[i].Item2, StaticVariables.upgradeCost);
        StaticVariables.increaseUpgradeLevel(upgradeDict[i]);
    }


}

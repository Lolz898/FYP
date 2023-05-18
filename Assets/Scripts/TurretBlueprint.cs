using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    public GameObject prefab;
    public int fCost;
    public int bCost;
    public int sCost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

    public int GetFleshSellAmount()
    {
        return fCost / 2;
    }

    public int GetBonesSellAmount()
    {
        return bCost / 2;
    }

    public int GetSoulsSellAmount()
    {
        return sCost / 2;
    }
}

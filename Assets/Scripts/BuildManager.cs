using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject standardTurretPrefab;
    public GameObject missileLauncherPrefab;

    private TurretBlueprint turretToBuild;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than 1 BuildManager in scene.");
            return;
        }
        instance = this;
    }

    public bool CanBuild
    {
        get { return turretToBuild != null;}
    }

    public bool HasMoney
    {
        get { return PlayerStats.Money >= turretToBuild.cost; }
    }

    public void BuildTurretOn(Node node)
    {
        if(PlayerStats.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerStats.Money -= turretToBuild.cost;
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        Debug.Log("Turret built, money left: " + PlayerStats.Money);
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}

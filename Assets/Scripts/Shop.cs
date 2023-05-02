using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint wall;
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectWall()
    {
        Debug.Log("Wall selected");
        buildManager.SelectTurretToBuild(wall);
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard turret selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {   
        Debug.Log("Missile Launcher selected");
        buildManager.SelectTurretToBuild(missileLauncher);
    }

    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer selected");
        buildManager.SelectTurretToBuild(laserBeamer);
    }
}

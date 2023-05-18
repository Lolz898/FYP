using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Constructs Type")]
    public TurretBlueprint resTower;
    public TurretBlueprint fleshGolem;
    public TurretBlueprint plagueSpreader;
    [Header("Bone Legion Type")]
    public TurretBlueprint skeletonArcherTower;
    public TurretBlueprint lichTower;
    public TurretBlueprint boneRattlingGunTower;
    [Header("Tortured Cult Type")]
    public TurretBlueprint ghostMageTower;
    public TurretBlueprint totemTower;
    public TurretBlueprint witchCoven;

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Put all construct functions here
    public void SelectResurrectTower()
    {
        Debug.Log("Resurrect Tower selected");
        buildManager.SelectTurretToBuild(resTower);
    }

    public void SelectFleshGolem()
    {
        Debug.Log("Flesh Golem selected");
        buildManager.SelectTurretToBuild(fleshGolem);
    }

    public void SelectPlagueSpreader()
    {
        Debug.Log("Plague Spreader selected");
        buildManager.SelectTurretToBuild(plagueSpreader);
    }

    // Put all bone legion functions here
    public void SelectSkeletonArcherTower()
    {
        Debug.Log("Skeleton Archer Tower selected");
        buildManager.SelectTurretToBuild(skeletonArcherTower);
    }

    public void SelectLichTower()
    {
        Debug.Log("Lich Tower selected");
        buildManager.SelectTurretToBuild(lichTower);
    }

    public void SelectBoneRattlingGun()
    {
        Debug.Log("Bone Rattling Gun selected");
        buildManager.SelectTurretToBuild(boneRattlingGunTower);
    }

    // Put all tortured cult functions here
    public void SelectGhostMageTower()
    {
        Debug.Log("Resurrect Tower selected");
        buildManager.SelectTurretToBuild(ghostMageTower);
    }

    public void SelectTotemTower()
    {
        Debug.Log("Totem Tower selected");
        buildManager.SelectTurretToBuild(totemTower);
    }

    public void SelectWitchCoven()
    {
        Debug.Log("Resurrect Tower selected");
        buildManager.SelectTurretToBuild(witchCoven);
    }
}

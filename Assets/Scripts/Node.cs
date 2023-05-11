using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color nodeColor;
    public Color notEnoughFleshColor;
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Flesh < blueprint.cost)
        {
            Debug.Log("Not enough flesh");
            return;
        }

        PlayerStats.Flesh -= blueprint.cost;
        GameObject _turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.Euler(35f, 0f, 0f));
        turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        buildManager.ClearTurretToBuild();
        rend.material.color = startColor;

        Debug.Log("Turret built");
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Flesh < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough flesh");
            return;
        }

        PlayerStats.Flesh -= turretBlueprint.upgradeCost;

        // Destroy old turret
        Destroy(turret);
        // Building upgraded turret
        GameObject _turret = Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;

        Debug.Log("Turret upgraded");
    }

    public void SellTurret()
    {
        PlayerStats.Flesh += turretBlueprint.GetSellAmount();

        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;

        rend.material.color = startColor;
    }

    public void ToggleNodeColor()
    {
        if (turret != null)
        {
            return;
        }

        if (rend.material.color == startColor)
        {
            rend.material.color = nodeColor;
        }
        else
        {
            rend.material.color = startColor;
        }
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        if (turret != null)
        {
            return;
        }

        if (buildManager.HasFlesh)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughFleshColor;
        }
    }

    private void OnMouseExit()
    {
        if (turret != null)
        {
            return;
        }

        if (rend.material.color != startColor)
        {
            rend.material.color = nodeColor;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation.Editor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject buildEffect;
    public GameObject sellEffect;

    public NodeUI nodeUI;

    public List<GameObject> nodes;

    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than 1 BuildManager in scene.");
            return;
        }
        instance = this;

        nodes.AddRange(GameObject.FindGameObjectsWithTag("Nodes"));
    }

    public bool CanBuild
    {
        get { return turretToBuild != null; }
    }

    public bool HasFlesh
    {
        get { return PlayerStats.Flesh >= turretToBuild.cost; }
    }


    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }

        if (turretToBuild != null)
        {
            ToggleAllNodesColor();
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        if (turretToBuild == null)
        {
            turretToBuild = turret;
            ToggleAllNodesColor();
        }
        else if (turret.prefab == turretToBuild.prefab)
        {
            turretToBuild = null;
            ToggleAllNodesColor();
        }
        else
        {
            turretToBuild = turret;
        }

        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void ClearTurretToBuild()
    {
        turretToBuild = null;

        ToggleAllNodesColor();
    }

    public void ToggleAllNodesColor()
    {
        foreach (GameObject node in nodes)
        {
            Node nComponent = node.GetComponent<Node>();
            if (nComponent != null)
            {
                nComponent.ToggleNodeColor();
            }
        }
    }
}

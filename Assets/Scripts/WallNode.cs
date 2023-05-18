using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class WallNode : MonoBehaviour
{
    public Color hoverColor;
    public Color nodeColor;
    public Color notEnoughResourcesColor;
    public Vector3 positionOffset;

    private Renderer rend;
    private Color startColor;

    private Transform t;

    BuildManager buildManager;

    public int wallCost = 100;

    public GameObject wallPrefab;
    public bool hasWall;
    public GameObject wallNode;

    private void Start()
    {
        buildManager = BuildManager.instance;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        t = GetComponent<Transform>();
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

        if (hasWall == true)
        {
            return;
        }

        BuildWall();
    }

    public void BuildWall()
    {
        if (PlayerStats.Bones < wallCost)
        {
            Debug.Log("Not enough bones");
            return;
        }

        PlayerStats.Bones -= wallCost;
        GameObject _wall = Instantiate(wallPrefab, transform.position + Vector3.up, Quaternion.identity);
        _wall.transform.SetParent(t);

        GameObject effect = Instantiate(buildManager.buildEffect, transform.position + Vector3.up, Quaternion.identity);
        Destroy(effect, 5f);

        wallNode = _wall.GetComponentInChildren<Node>().gameObject;

        buildManager.nodes.Add(wallNode);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (PlayerStats.Bones > wallCost)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughResourcesColor;
        }
    }

    private void OnMouseExit()
    {
        if (rend.material.color != startColor)
        {
            rend.material.color = nodeColor;
        }
    }

}

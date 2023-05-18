using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public GameObject rangeSphere;

    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellAmount;
    public Button upgradeButton;

    private Node target;
    private Turret turret;

    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        if (!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "MAXED";
            upgradeButton.interactable = false;
        }

        sellAmount.text = GetSellAmount();

        ui.SetActive(true);
        // Get the range of the turret on the target node
        turret = target.turretBlueprint.prefab.GetComponent<Turret>();
        if (turret != null )
        {
            // Set the range sphere size equal to the range
            Vector3 scale = new Vector3(turret.range * 2f, 0.1f, turret.range * 2f);
            rangeSphere.transform.localScale = scale;
            rangeSphere.SetActive(true);
        }
    }

    private string GetSellAmount()
    {
        string sellText = null;

        if (target.turretBlueprint.GetFleshSellAmount() > 0)
        {
            sellText += "F" + target.turretBlueprint.GetFleshSellAmount() + " ";
        }

        if (target.turretBlueprint.GetBonesSellAmount() > 0)
        {
            sellText += "B" + target.turretBlueprint.GetBonesSellAmount() + " ";
        }

        if (target.turretBlueprint.GetSoulsSellAmount() > 0)
        {
            sellText += "S" + target.turretBlueprint.GetSoulsSellAmount();
        }

        return sellText;
    }

    public void Hide()
    {
        ui.SetActive(false);
        rangeSphere.SetActive(false);
        turret = null;
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
        target.isUpgraded = false;
    }

}

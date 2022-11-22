using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "$" + PlayerStats.Money.ToString();
    }
}

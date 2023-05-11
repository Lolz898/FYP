using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulsUI : MonoBehaviour
{
    public TextMeshProUGUI soulsText;

    private void Update()
    {
        soulsText.text = "SOULS: " + PlayerStats.Souls.ToString();
    }
}

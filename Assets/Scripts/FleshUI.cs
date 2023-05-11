using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FleshUI : MonoBehaviour
{
    public TextMeshProUGUI fleshText;

    private void Update()
    {
        fleshText.text = "FLESH: " + PlayerStats.Flesh.ToString();
    }
}

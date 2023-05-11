using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonesUI : MonoBehaviour
{
    public TextMeshProUGUI bonesText;

    private void Update()
    {
        bonesText.text = "BONES: " + PlayerStats.Bones.ToString();
    }
}

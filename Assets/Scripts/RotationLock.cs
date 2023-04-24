using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{
    float XLock = 60;
    float lockPos = 0;

    void Update()
    {
        transform.rotation = Quaternion.Euler(XLock, lockPos, lockPos);
    }
}

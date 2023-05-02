using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.Rotate(45, 0, 0);
    }

}

using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    public Sprite[] enemySprites;
    public SpriteRenderer spriteRenderer;
    public float animationSpeed = 0.2f;
    private int currentSprite = 0;
    private float timeSinceLastFrame = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.Rotate(45, 0, 0);
    }

    private void Update()
    {
        if (spriteRenderer != null)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= animationSpeed)
            {
                timeSinceLastFrame = 0;
                currentSprite = (currentSprite + 1) % enemySprites.Length;
                spriteRenderer.sprite = enemySprites[currentSprite];
            }
            spriteRenderer.transform.rotation = Quaternion.Euler(45, 0, 0);
        }
    }
}

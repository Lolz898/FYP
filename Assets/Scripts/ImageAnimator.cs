using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageAnimator : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;

    public float animationSpeed = 0.2f;
    private int currentSprite = 0;
    private float timeSinceLastFrame = 0;

    private void Update()
    {
        if (image != null)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= animationSpeed)
            {
                timeSinceLastFrame = 0;
                currentSprite = (currentSprite + 1) % sprites.Length;
                image.sprite = sprites[currentSprite];
            }
        }
    }

}

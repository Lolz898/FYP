using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        // Get all the buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>();

        // Add click sound to each button
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}

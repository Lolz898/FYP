using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive = 0;

    public Wave[] waves;
    public Transform spawnPoint;

    public TextMeshProUGUI waveCountdownText;

    public GameManager gameManager;

    public float waveTimer = 5f;

    private float countdown = 2f;
    private int waveIndex = 0;

    private void Start()
    {
        enemiesAlive = 0;
    }

    void Update()
    {
        if (enemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            enabled = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = waveTimer;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = "NEXT WAVE: " + string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        enemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
    }


    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, enemy.transform.rotation);
    }

}

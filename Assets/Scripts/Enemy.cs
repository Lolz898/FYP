using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Statistics")]
    public float startSpeed = 6f;
    public float startHealth = 100;
    public float sightRange = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    public float attackDamage = 10f;
    public float lifeTimer = 0f;

    [HideInInspector]
    public float lastAttackTime;

    [Header("NavMesh")]
    // 0 = Node based, 1 = NavMesh based
    public int pathingType = 0;
    public NavMeshAgent agent;
    public bool isAlly = false;
    public bool fightPlayerUnits = false;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float health;
    public int fWorth = 50;
    public int bWorth = 0;
    public int sWorth = 0;

    public GameObject deathEffect;
    private bool isDead;

    public bool isZombie;

    public bool spawnUnitsOnDeath;
    public GameObject unitToSpawn;
    public int unitToSpawnAmount;

    [Header("Unity Setup")]
    public Image healthBar;
    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip spawnSound;
    public AudioClip attackSound;
    public AudioClip deathSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (spawnSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(spawnSound, 0.4f);
        }

        speed = startSpeed;
        if (pathingType == 1)
        {
            agent.speed = startSpeed;
        }
        health = startHealth;
        isDead = false;

        if (lifeTimer > 0f)
        {
            StartCoroutine(LifeTimer());
        }
    }

    private void Update()
    {
        if (pathingType == 1)
        {
            agent.speed = speed;
        }
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTimer);
        if (!isDead)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        // Apply inDamageReduction from all active UnitModifier components
        float damageReductionModifier = 1.0f;
        UnitModifier[] activeModifiers = GetComponentsInChildren<UnitModifier>();
        foreach (UnitModifier modifier in activeModifiers)
        {
            damageReductionModifier *= (1.0f - modifier.inDamageReduction);
        }

        // Apply damage with the reduced damage amount
        float actualDamageAmount = amount * damageReductionModifier;

        health -= actualDamageAmount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead) 
        {
            Die();
        }
    }

    public void ApplyModifier(UnitModifier _modifier)
    {
        UnitModifier modifier = Instantiate(_modifier, transform);
    }

    void Die()
    {
        isDead = true;
        PlayerStats.Flesh += fWorth;
        PlayerStats.Bones += bWorth;
        PlayerStats.Souls += sWorth;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);

        if (deathSound != null)
        {
            GameObject audioObject = new GameObject("DeathSoundObject"); // Create a new GameObject
            AudioSource _audioSource = audioObject.AddComponent<AudioSource>(); // Add an AudioSource component
            _audioSource.spatialBlend = 1f; // Set spatial blend to 3D
            _audioSource.playOnAwake = false; // Disable play on awake

            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(deathSound, 0.6f);

            // Destroy the audio object after the clip has finished playing
            Destroy(audioObject, deathSound.length + 0.3f);
        }

        if (!isAlly)
        {
            WaveSpawner.enemiesAlive--;
        }

        if (spawnUnitsOnDeath)
        {
            for (int i = 0; i < unitToSpawnAmount; i++)
            {
                Instantiate(unitToSpawn, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

}

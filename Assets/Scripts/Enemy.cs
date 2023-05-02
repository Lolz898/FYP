using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 6f;
    public float startHealth = 100;

    [Header("NavMesh")]
    // 0 = Node based, 1 = NavMesh based
    public int pathingType = 0;
    public NavMeshAgent agent;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float health;
    public int worth = 50;

    public GameObject deathEffect;
    private bool isDead;

    [Header("Unity Stuff")]
    public Image healthBar;

    private void Start()
    {
        speed = startSpeed;
        if (pathingType == 1)
        {
            agent.speed = startSpeed;
        }
        health = startHealth;
        isDead = false;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead) 
        {
            Die();
        }
    }

    public void Slow(float amount)
    {
        if (pathingType == 0)
        {
            speed = startSpeed * (1f - amount);
        }
        else if (pathingType == 1)
        {
            agent.speed = startSpeed * (1f - amount);
        }
    }

    void Die()
    {
        isDead = true;
        PlayerStats.Money += worth;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        WaveSpawner.enemiesAlive--;

        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 6f;
    [HideInInspector]
    public float speed;
    public float health = 100;
    public int worth = 50;

    public GameObject deathEffect;

    private bool enemyDeathOnce;

    private void Start()
    {
        speed = startSpeed;
        enemyDeathOnce = false;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0) 
        {
            if (!enemyDeathOnce)
            {
                Die();
            }
        }
    }

    public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount); 
    }

    void Die()
    {
        enemyDeathOnce = true;
        PlayerStats.Money += worth;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);
    }

}

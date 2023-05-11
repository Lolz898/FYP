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

    //[HideInInspector]
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
        Debug.Log(speed);
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
        PlayerStats.Flesh += worth;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        if (!isAlly)
        {
            WaveSpawner.enemiesAlive--;
        }

        Destroy(gameObject);
    }

}

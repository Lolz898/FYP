using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    private Enemy targetAlly;
    private int waypointIndex = 0;

    private GameObject startPoint;
    private GameObject endPoint;
    public NavMeshAgent agent;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        enemy.lastAttackTime = -enemy.attackCooldown;

        startPoint = GameObject.Find("Start Point");
        endPoint = GameObject.Find("End Point");

        if (enemy.pathingType == 0)
        {
            target = Waypoints.waypoints[0];
        }
        else if (enemy.pathingType == 1 && enemy.isAlly == false)
        {
            target = endPoint.transform;
            agent.SetDestination(target.position);
        }
        else if (enemy.pathingType == 1 && enemy.isAlly == true)
        {
            target = startPoint.transform;
            agent.SetDestination(target.position);
        }
    }

    public void ResetStats()
    {
        enemy = GetComponent<Enemy>();

        enemy.lastAttackTime = -enemy.attackCooldown;

        waypointIndex = 0;

        if (enemy.pathingType == 0)
        {
            target = Waypoints.waypoints[0];
        }
        else if (enemy.pathingType == 1 && enemy.isAlly == false)
        {
            target = endPoint.transform;
            agent.SetDestination(target.position);
        }
        else if (enemy.pathingType == 1 && enemy.isAlly == true)
        {
            target = startPoint.transform;
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
        if (enemy.pathingType == 0)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * (enemy.speed * Time.deltaTime), Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.4f)
            {
                GetNextWaypoint();
            }
        }
        else if (enemy.pathingType == 1)
        {
            if (!enemy.isAlly)
            {
                if (Vector3.Distance(transform.position, endPoint.transform.position) <= 2f)
                {
                    EndPath();
                    return;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, startPoint.transform.position) <= 2f)
                {
                    if (enemy.fWorth > 0 || enemy.bWorth > 0 || enemy.sWorth > 0)
                    {
                        PlayerStats.Flesh += enemy.fWorth;
                        PlayerStats.Bones += enemy.bWorth;
                        PlayerStats.Souls += enemy.sWorth;
                    }
                    EndPath();
                    return;
                }
            }

            if (enemy.isAlly)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                float nearestDistance = Mathf.Infinity;

                foreach (GameObject enemyObject in enemies)
                {
                    float distance = Vector3.Distance(transform.position, enemyObject.transform.position);

                    if (distance <= enemy.sightRange && distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        targetEnemy = enemyObject.GetComponent<Enemy>();
                    }
                }

                if (targetEnemy != null)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.transform.position);

                    if (distanceToEnemy > enemy.sightRange)
                    {
                        agent.SetDestination(startPoint.transform.position);
                        targetEnemy = null;
                        return;
                    }
                    
                    if (distanceToEnemy > enemy.attackRange)
                    {
                        agent.SetDestination(targetEnemy.transform.position);
                    }
                    
                    if ((distanceToEnemy <= enemy.attackRange) && (Time.time > enemy.lastAttackTime + enemy.attackCooldown))
                    {
                        // Attack the enemy if in range and the attack cooldown has passed
                        targetEnemy.TakeDamage(enemy.attackDamage);
                        enemy.lastAttackTime = Time.time;

                        if (enemy.attackSound != null)
                        {
                            enemy.audioSource.pitch = Random.Range(0.9f, 1.1f);
                            enemy.audioSource.PlayOneShot(enemy.attackSound, 0.4f);
                        }

                        agent.SetDestination(targetEnemy.transform.position);
                    }
                }
                else
                {
                    // If there's no target enemy, move back to start location
                    agent.SetDestination(startPoint.transform.position);
                    targetEnemy = null;
                }
            }
            else if (enemy.fightPlayerUnits)
            {
                GameObject[] allies = GameObject.FindGameObjectsWithTag("Allies");
                float nearestDistance = Mathf.Infinity;

                foreach (GameObject allyObject in allies)
                {
                    float distance = Vector3.Distance(transform.position, allyObject.transform.position);

                    if (distance <= enemy.sightRange && distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        targetAlly = allyObject.GetComponent<Enemy>();
                    }
                }

                if (targetAlly != null)
                {
                    float distanceToAlly = Vector3.Distance(transform.position, targetAlly.transform.position);

                    if (distanceToAlly > enemy.sightRange)
                    {
                        agent.SetDestination(endPoint.transform.position);
                        targetAlly = null;
                        return;
                    }

                    if (distanceToAlly > enemy.attackRange)
                    {
                        agent.SetDestination(targetAlly.transform.position);
                    }

                    if ((distanceToAlly <= enemy.attackRange) && (Time.time > enemy.lastAttackTime + enemy.attackCooldown))
                    {
                        // Attack the enemy if in range and the attack cooldown has passed
                        targetAlly.TakeDamage(enemy.attackDamage);
                        enemy.lastAttackTime = Time.time;

                        if (enemy.attackSound != null)
                        {
                            enemy.audioSource.pitch = Random.Range(0.9f, 1.1f);
                            enemy.audioSource.PlayOneShot(enemy.attackSound, 0.4f);
                        }

                        agent.SetDestination(targetAlly.transform.position);
                    }
                }
                else
                {
                    // If there's no target enemy, move back to start location
                    agent.SetDestination(endPoint.transform.position);
                    targetAlly = null;
                }
            }
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            EndPath();
            return;
        }

        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];
    }

    void EndPath()
    {
        if (!enemy.isAlly)
        {
            PlayerStats.Lives--;
            WaveSpawner.enemiesAlive--;
        }
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int waypointIndex = 0;

    private GameObject endPoint;
    public NavMeshAgent agent;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        endPoint = GameObject.Find("End Point");

        if (enemy.pathingType == 0)
        {
            target = Waypoints.waypoints[0];
        }
        else if (enemy.pathingType == 1)
        {
            target = endPoint.transform;
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

            enemy.speed = enemy.startSpeed;
        }
        else if (enemy.pathingType == 1)
        {
            agent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) <= 1f)
            {
                EndPath();
                return;
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
        PlayerStats.Lives--;
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }

}

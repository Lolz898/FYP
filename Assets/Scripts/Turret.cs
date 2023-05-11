using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Turret : MonoBehaviour
{
    [Header("General")]
    public float range = 15f;
    public Sprite[] towerSprites;
    public SpriteRenderer spriteRenderer;
    public float animationSpeed = 0.2f;
    private int currentSprite = 0;
    private float timeSinceLastFrame = 0;

    [Header("Bullets")]
    public bool useBullets = true;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [Header("Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowAmount = 0.5f;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Resurrect Zombie")]
    public bool useResZombie = false;
    public bool canSpawn = true;
    public float spawnRate = 3f;
    public GameObject zombiePrefab;
    private bool spawning = false;

    [Header("All Modifier Prefabs")]
    public bool useNormalSlow = false;
    public UnitModifier normalSlow;
    public bool useNormalPoison = false;
    public UnitModifier normalPoison;

    [Header("Unity Setup Fields")]
    public float turnSpeed = 10f;
    public string enemyTag = "Enemy";

    private Transform target;
    private Enemy targetEnemy;
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= animationSpeed)
            {
                timeSinceLastFrame = 0;
                currentSprite = (currentSprite + 1) % towerSprites.Length;
                spriteRenderer.sprite = towerSprites[currentSprite];
            }
        }

        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
        }

        if (useLaser)
        {
            Laser();
        }
        else if (useBullets)
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
        else if (useResZombie)
        {
            if (!spawning)
            {
                spawning = true;
                StartCoroutine(SpawnZombie());
            }
        }
    }

    IEnumerator SpawnZombie()
    {
        while (canSpawn)
        {
            GameObject nearestGround = null;
            float nearestDistance = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Ground"))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestGround = collider.gameObject;
                    }
                }
            }

            if (nearestGround != null)
            {
                Instantiate(zombiePrefab, nearestGround.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        ApplyAllModifiers(targetEnemy);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }    

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation, transform);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void ApplyAllModifiers(Enemy enemy)
    {
        if (useNormalPoison)
        {
            enemy.ApplyModifier(normalPoison);
        }

        if (useNormalSlow)
        {
            enemy.ApplyModifier(normalSlow);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

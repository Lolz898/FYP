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

    [Header("Create Flesh Golem")]
    public bool useFleshGolem = false;
    public bool canSpawnFGolem = true;
    public GameObject fleshGolemPrefab;
    public bool consuming = false;
    public int allyAmount = 0;

    [Header("Tower AoE")]
    public string targetTag;
    public bool useTowerAoE = false;
    public int AoEDamage = 5;
    public float AoERate = 0f;
    public int soulCost = 0;
    private bool AoEActive = false;

    [Header("All Modifier Prefabs")]
    public bool useNormalSlow = false;
    public UnitModifier normalSlow;
    public bool useWeakFire = false;
    public UnitModifier weakFire;
    public bool useSoulMark = false;
    public UnitModifier soulMark;
    public bool useZombiePlague = false;
    public UnitModifier zombiePlague;
    public bool useAttackSpeedUp = false;
    public UnitModifier attackSpeedUp;
    public bool useResistanceDown = false;
    public UnitModifier resistanceDown;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public AudioSource audioSource;
    public AudioClip projAttackSound;
    public AudioClip aoeAttackSound;

    private Transform target;
    private Enemy targetEnemy;
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
        
        if (useBullets)
        {
            if (fireCountdown <= 0f)
            {
                if (target != null) 
                {
                    Shoot();
                    fireCountdown = 1f / fireRate;
                }
            }

            fireCountdown -= Time.deltaTime;
        }
        
        if (useResZombie)
        {
            if (!spawning)
            {
                spawning = true;
                StartCoroutine(SpawnZombie());
            }
        }

        if (useFleshGolem)
        {
            if (!consuming)
            {
                consuming = true;
                StartCoroutine(SpawnFleshGolem());
            }
        }

        if (useTowerAoE)
        {
            if (!AoEActive)
            {
                AoEActive = true;
                StartCoroutine(towerAoE());
            }
        }
    }

    IEnumerator SpawnZombie()
    {
        while (canSpawn)
        {
            if (PlayerStats.Flesh > 15)
            {
                PlayerStats.Flesh -= 15;
                Instantiate(zombiePrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator SpawnFleshGolem()
    {
        while (canSpawnFGolem)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);

            foreach (Collider collider in colliders)
            {
                Enemy unit = collider.GetComponent<Enemy>();
                if (unit != null && unit.isZombie)
                {
                    unit.TakeDamage(unit.health);
                    allyAmount++;
                }
            }

            if (allyAmount >= 5)
            {
                Instantiate(fleshGolemPrefab, transform.position, Quaternion.identity);
                allyAmount = 0;
            }

            yield return null;
        }
    }

    IEnumerator towerAoE()
    {
        while (true)
        {
            if (PlayerStats.Souls > soulCost)
            {
                yield return new WaitForSeconds(AoERate);
                
                audioSource.PlayOneShot(aoeAttackSound, 0.6f);

                PlayerStats.Souls -= soulCost;

                Collider[] colliders = Physics.OverlapSphere(transform.position, range);

                List<Enemy> targets = new List<Enemy>();

                foreach (Collider collider in colliders)
                {
                    Enemy unit = collider.GetComponent<Enemy>();
                    if (unit != null && unit.CompareTag(targetTag))
                    {
                        targets.Add(unit);
                    }
                }

                foreach (Enemy target in targets)
                {
                    ApplyAllModifiers(target);
                    target.TakeDamage(AoEDamage);;
                }
            }
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

        if (projAttackSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(projAttackSound, 0.6f);
        }

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void ApplyAllModifiers(Enemy enemy)
    {
        if (useNormalSlow)
        {
            enemy.ApplyModifier(normalSlow);
        }
        
        if (useWeakFire)
        {
            enemy.ApplyModifier(weakFire);
        }

        if (useSoulMark)
        {
            enemy.ApplyModifier(soulMark);
        }

        if (useZombiePlague)
        {
            enemy.ApplyModifier(zombiePlague);
        }

        if (useAttackSpeedUp)
        {
            enemy.ApplyModifier(attackSpeedUp);
        }

        if (useResistanceDown)
        {
            enemy.ApplyModifier(resistanceDown);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

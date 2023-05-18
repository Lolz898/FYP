using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModifier : MonoBehaviour
{
    public float effectDuration = 0.0f;

    public float speedAmount = 0.0f;
    private float speedModifier;

    public float attackSpeedAmount = 0.0f;
    private float attackSpeedModifier;

    public float damageAmount = 0.0f;
    public float damageRate = 0.0f;

    public float inDamageReduction = 0.0f;

    public bool isFire = false;
    private int startingFWorth;
    private int startingBWorth;
    private int startingSWorth;
    public bool dropAdditionalSoul = false;
    public bool spawnUnitOnDeath = false;
    public GameObject unitToSpawnOnDeath;

    private Enemy unit;

    public AudioSource audioSource;
    public AudioClip loopingEffectSound;


    void Start()
    {
        unit = GetComponentInParent<Enemy>();

        startingFWorth = unit.fWorth;
        startingBWorth = unit.bWorth;
        startingSWorth = unit.sWorth;

        audioSource = GetComponentInParent<AudioSource>();
        audioSource.clip = loopingEffectSound;
        audioSource.loop = true;

        if (loopingEffectSound != null && audioSource.isPlaying == false && audioSource.enabled)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }

        if (effectDuration > 0.0f)
        {
            StartCoroutine(DestroyModifier());
        }

        if (isFire && unit.fWorth > 0)
        {
            int newWorth = unit.fWorth / 4;
            unit.fWorth -= newWorth;
            unit.bWorth += newWorth;
        }

        if (dropAdditionalSoul)
        {
            unit.sWorth += 1;
        }

        if (spawnUnitOnDeath)
        {
            unit.spawnUnitsOnDeath = true;
            unit.unitToSpawn = unitToSpawnOnDeath;
            unit.unitToSpawnAmount += 1;
        }

        if (damageAmount > 0.0f || damageAmount < 0.0f)
        {
            StartCoroutine(DamageOverTime());
        }

        if (speedAmount > 0.0f || speedAmount < 0.0f)
        {
            StartCoroutine(ChangeSpeed());
        }

        if (attackSpeedAmount > 0.0f || attackSpeedAmount < 0.0f)
        {
            StartCoroutine(ChangeAttackSpeed());
        }
    }

    public void RevertModifiers()
    {
        if (spawnUnitOnDeath)
        {
            unit.unitToSpawn = null;
            unit.unitToSpawnAmount--;
        }
        Debug.Log("deleted modifier");

        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Destroy(gameObject);
    }

    private IEnumerator DestroyModifier()
    {
        yield return new WaitForSeconds(effectDuration + 0.3f);
        audioSource.Stop();
        Destroy(gameObject);
    }
    
    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            unit.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageRate);
        }
    }

    private IEnumerator ChangeSpeed()
    {
        speedModifier = 1f + (speedAmount / 100f); // Convert speedAmount to a percentage and add 1 to get a modifier value
        unit.speed *= speedModifier; // Adjust the unit's speed by the modifier

        yield return new WaitForSeconds(effectDuration);

        // Reverse the speed adjustment to restore the unit's original speed
        unit.speed /= speedModifier;

    }

    private IEnumerator ChangeAttackSpeed()
    {
        attackSpeedModifier = 1f + (attackSpeedAmount / 100f); // Convert speedAmount to a percentage and add 1 to get a modifier value
        unit.attackCooldown *= attackSpeedModifier; // Adjust the unit's attack speed by the modifier

        yield return new WaitForSeconds(effectDuration);
        
        // Reverse the speed adjustment to restore the unit's original attack speed
        unit.attackCooldown /= attackSpeedModifier;
        
    }

}

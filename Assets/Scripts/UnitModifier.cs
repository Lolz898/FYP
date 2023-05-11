using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModifier : MonoBehaviour
{
    public float effectDuration = 0.0f;
    public float speedAmount = 0.0f;
    public float damageAmount = 0.0f;
    public float damageRate = 0.0f;
    public float inDamageReduction = 0.0f;

    private Enemy unit;


    void Start()
    {
        unit = GetComponentInParent<Enemy>();
        StartCoroutine(DestroyModifier());

        if (damageAmount > 0.0f)
        {
            StartCoroutine(DamageOverTime());
        }

        if (speedAmount > 0.0f)
        {
            StartCoroutine(ChangeSpeed());
        }
    }

    private IEnumerator DestroyModifier()
    {
        yield return new WaitForSeconds(effectDuration);
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
        float speedModifier = 1f + (speedAmount / 100f); // Convert speedAmount to a percentage and add 1 to get a modifier value
        unit.speed *= speedModifier; // Adjust the unit's speed by the modifier

        try
        {
            yield return new WaitForSeconds(effectDuration);
        }
        finally
        {
            // Reverse the speed adjustment to restore the unit's original speed
            unit.speed /= speedModifier;
        }
    }

}

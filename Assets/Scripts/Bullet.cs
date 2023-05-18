using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage = 50;
    public float explosionRadius = 0f;
    public GameObject impactEffect;
    public Turret parentTurret;

    public AudioSource audioSource;
    public AudioClip hitSound;

    public void Start()
    {
        parentTurret = GetComponentInParent<Turret>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 3f);

        if (hitSound != null)
        {
            GameObject audioObject = new GameObject("DeathSoundObject"); // Create a new GameObject
            AudioSource _audioSource = audioObject.AddComponent<AudioSource>(); // Add an AudioSource component
            _audioSource.spatialBlend = 1f; // Set spatial blend to 3D
            _audioSource.playOnAwake = false; // Disable play on awake

            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(hitSound, 0.6f);

            // Destroy the audio object after the clip has finished playing
            Destroy(audioObject, hitSound.length);
        }

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null) { e.TakeDamage(damage); }

        parentTurret.ApplyAllModifiers(e);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

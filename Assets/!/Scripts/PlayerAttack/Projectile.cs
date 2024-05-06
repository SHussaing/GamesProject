using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
    public bool destroyOnHit;
    public float timeToDestroy;

    [Header("Effect")]
    public GameObject hitEffect;
    public GameObject muzzleEffect;

    [Header("Explosive Projectile")]
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public int explosionDamage;
    public GameObject explosionEffect;

    private Rigidbody rb;

    private bool hitTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (muzzleEffect != null)
            Instantiate(muzzleEffect, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitTarget)
            return;
        else
            hitTarget = true;

        // check if you hit an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ZombieStats stats = collision.gameObject.GetComponent<ZombieStats>();

            stats.TakeDamage(damage);

            if (isExplosive)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

                foreach (Collider nearbyObject in colliders)
                {
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }

                    if (nearbyObject.CompareTag("Enemy"))
                    {
                        ZombieStats enemyStats = nearbyObject.GetComponent<ZombieStats>();

                        enemyStats.TakeDamage(explosionDamage);
                    }
                }

                if (explosionEffect != null)
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            else
            {
                if (hitEffect != null)
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            if (destroyOnHit)
                Destroy(gameObject);
        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);

        //destroy the object after 1 second
        Destroy(gameObject, timeToDestroy);
    }
}
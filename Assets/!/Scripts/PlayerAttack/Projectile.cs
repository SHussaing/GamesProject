using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource explosionSound;

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
        //instansiate explosion sound from the game object explosion sound in the game scene hierarchy
        explosionSound = GameObject.Find("explosionSound").GetComponent<AudioSource>();

        if (muzzleEffect != null)
            Instantiate(muzzleEffect, transform.position, Quaternion.identity);
        
    }

    private void Update()
    {
        Destroy(gameObject, 10f);
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

        }

        if (!destroyOnHit)
        {
            //destroy the object after time
            Destroy(gameObject, timeToDestroy);
        }


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

            if (explosionSound != null)
            {
               explosionSound.Play();
            }

        }
        else
        {
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);

        if (destroyOnHit)
            Destroy(gameObject);
    }


}

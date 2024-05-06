using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public int damage;
    public bool destroyOnHit;
    public float timeToDestroy;

    private Rigidbody rb;

    private bool hitTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    [SerializeField] private int damage;
    public float attackspeed;

    [SerializeField] private bool canAttack;


    private void Start()
    {
        InitVariables();
    }


    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage);
    } 
    
    public override void Die()
    {
    
        base.Die();
        Destroy(gameObject);

    }

    public override void InitVariables()
    {

        maxHealth = 25;
        SetHealthTo(maxHealth);
        damage = 10;
        attackspeed = 1.5f;
        canAttack = true;
        isDead = false;

    }


}

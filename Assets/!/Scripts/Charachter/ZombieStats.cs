using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStats : CharacterStats
{
    public int damage;
    public float attackspeed;
    private Animator anim = null;
    private Rigidbody rb = null;
    private NavMeshAgent agent = null;
    private ZombieController controller = null;
    private CapsuleCollider capsuleCollider = null;
    public int CoinToDrop ;
    [SerializeField] private PlayerStats player;
    /*[SerializeField] private bool canAttack;*/


    private void Start()
    {
        /*InitVariables();*/
        GetReferences();
        
        
    }


    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage);
    } 
    
    public override void Die()
    {

        player.AddCoins(CoinToDrop);
        //disable the collider
        capsuleCollider.enabled = false;
        anim.SetTrigger("die");
        //destroy the object after 2 seconds
        base.Die();
        Destroy(gameObject, 2f);
        //make sure the zombie doesn't move after death
        rb.isKinematic = true;
        //make sure the zombie doesn't move with the player
        transform.SetParent(null);
        agent.speed = 0;
        controller.enabled = false;
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<ZombieController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    /*    public override void InitVariables()
        {

            maxHealth = 25;
            SetHealthTo(maxHealth);
            damage = 10;
            attackspeed = 1.5f;
            canAttack = true;
            isDead = false;

        }*/


}

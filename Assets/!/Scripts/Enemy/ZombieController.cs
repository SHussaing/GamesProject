using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private float timeOfLastAttack = 0;
    private bool reachedPlayer = false;
    private NavMeshAgent agent = null;
    private Animator anim = null;
    private ZombieStats stats = null;
    [SerializeField] private Transform player;
    public float stoppingDistance = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
    }

    private void MoveToPlayer()
    {

        agent.SetDestination(player.position);
        RotateTowardsPlayer();
        anim.SetFloat("speed", 1f, 0.3f, Time.deltaTime);

        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        if (distanceToTarget <= stoppingDistance)
        {
            anim.SetFloat("speed", 0f);
            //Attack
            if (!reachedPlayer)
            {
                reachedPlayer = true;
                timeOfLastAttack = Time.time - 1.3f; // Decrease the delay by 0.5 seconds
            }


            if (Time.time >= timeOfLastAttack + stats.attackspeed)
            {
                timeOfLastAttack = Time.time;
                CharacterStats playerStats = player.GetComponent<CharacterStats>();
                AttackPlayer(playerStats);
            }

        }
        else
        {
            if (reachedPlayer)
            {
                reachedPlayer = false;
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        // Get the direction vector from the current position to the player's position
        Vector3 direction = player.position - transform.position;

        // Calculate the rotation only along the y-axis using LookRotation
        Quaternion yRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Apply the rotation to the GameObject, only modifying the y-axis
        transform.rotation = Quaternion.Euler(0f, yRotation.eulerAngles.y, 0f);
    }


    public void AttackPlayer(CharacterStats statsToDamage)
    {
        anim.SetTrigger("attack");
        stats.DealDamage(statsToDamage);
    }
}

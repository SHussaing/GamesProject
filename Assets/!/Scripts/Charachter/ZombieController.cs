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
    [SerializeField] public Transform player;
    public float stoppingDistance = 2f;

    [Header("Audio")]
    public AudioSource normalSound;
    public AudioSource attackSound;
    private float timer;

    // Start is called before the first frame update
    private void Start()
    {
        GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();

        // Decrement the timer with the time passed since the last frame
        timer -= Time.deltaTime;

        // Check if the timer has reached zero or less
        if (timer <= 0f)
        {
            //play normal sound depending on the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Calculate volume based on distance
            float volume = Mathf.Clamp01(1f - distanceToPlayer);

            if(normalSound != null)
            {
                // Play sound with adjusted volume
                normalSound.volume = volume;
                AudioSource.PlayClipAtPoint(normalSound.clip, transform.position);
            }
            // Reset the timer with a new random value between minTime and maxTime
            timer = Random.Range(2f, 8f);
        }
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(gameObject.name.StartsWith("Enemy_ZombieBasic"))
        {
            normalSound = GameObject.Find("zombieBasicSound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieBasicAttackSound").GetComponent<AudioSource>();
        } else if(gameObject.name.StartsWith("Enemy_ZombieArm"))
        {
            normalSound = GameObject.Find("zombieArmSound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieArmAttackSound").GetComponent<AudioSource>();
        }
        else if(gameObject.name.StartsWith("Enemy_ZombieRibcage"))
        {
            normalSound = GameObject.Find("zombieRibcageSound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieRibcageAttackSound").GetComponent<AudioSource>();
        }
        else if(gameObject.name.StartsWith("Enemy_ZombieChubby"))
        {
            normalSound = GameObject.Find("zombieChubbySound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieChubbyAttackSound").GetComponent<AudioSource>();
        }
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
                timeOfLastAttack = Time.time - 0.5f; // Decrease the delay by 0.5 seconds
            }


            // Calculate the time for the next attack based on the attack speed
            float nextAttackTime = timeOfLastAttack + 1f / stats.attackspeed;

            if (Time.time >= nextAttackTime)
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
        if (attackSound != null)
        {
            attackSound.Play();
        }
        stats.DealDamage(statsToDamage);
    }
}

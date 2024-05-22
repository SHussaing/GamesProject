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
    public float jumpHeight = 200000.0f;
    public float jumpCooldown = 2.0f;
    private bool isJumping = false;
    private float lastJumpTime = 0f;

    private Vector3 lastPosition;
    private float stuckCheckTimer = 2.0f;
    private float timeSinceLastMove = 0f;

    [Header("Audio")]
    public AudioSource normalSound;
    public AudioSource attackSound;
    private float timer;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        GetReferences();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
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

            if (normalSound != null)
            {
                // Play sound with adjusted volume
                normalSound.volume = volume;
                AudioSource.PlayClipAtPoint(normalSound.clip, transform.position);
            }
            // Reset the timer with a new random value between minTime and maxTime
            timer = Random.Range(2f, 8f);
        }

        // Check if the player is above and near the zombie
        if ((player.position.y > transform.position.y + 1f && Vector3.Distance(transform.position, player.position) <= stoppingDistance) ||
            IsStuck())
        {
            if (Time.time > lastJumpTime + jumpCooldown && !isJumping)
            {
                StartCoroutine(Jump());
            }
        }

        CheckStuck();
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (gameObject.name.StartsWith("Enemy_ZombieBasic"))
        {
            normalSound = GameObject.Find("zombieBasicSound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieBasicAttackSound").GetComponent<AudioSource>();
        }
        else if (gameObject.name.StartsWith("Enemy_ZombieArm"))
        {
            normalSound = GameObject.Find("zombieArmSound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieArmAttackSound").GetComponent<AudioSource>();
        }
        else if (gameObject.name.StartsWith("Enemy_ZombieRibcage"))
        {
            normalSound = GameObject.Find("zombieRibcageSound").GetComponent<AudioSource>();
            attackSound = GameObject.Find("zombieRibcageAttackSound").GetComponent<AudioSource>();
        }
        else if (gameObject.name.StartsWith("Enemy_ZombieChubby"))
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

    private IEnumerator Jump()
    {
        isJumping = true;
        agent.isStopped = true;
        rb.isKinematic = false;

        // Apply jump force
        rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);

        // Wait until the jump is over
        yield return new WaitForSeconds(1f);

        // Restore normal behavior
        rb.isKinematic = true;
        agent.isStopped = false;
        isJumping = false;
        lastJumpTime = Time.time;
    }

    private bool IsStuck()
    {
        return timeSinceLastMove >= stuckCheckTimer;
    }

    private void CheckStuck()
    {
        if (Vector3.Distance(transform.position, lastPosition) <= 2f)
        {
            timeSinceLastMove += Time.deltaTime;
        }
        else
        {
            timeSinceLastMove = 0f;
            lastPosition = transform.position;
        }
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

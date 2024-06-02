using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn System")]

    public GameObject[] zombiePrefabs; // Array of zombie prefabs to spawn
    public Transform player; // Assign the player transform in the inspector
    public float spawnRadius; // Radius within which zombies will spawn
    public float minDistanceFromPlayer; // Minimum distance from the player for spawning
    public int totalSpawnCount; // Total number of zombies to spawn
    public float timeBetweenSpawns; // Time between each spawn
    private int spawnCount; // Number of zombies currently in the scene

    //zombie prefabs
/*    [Header("Zombie Prefabs")]

    [SerializeField] GameObject ZombieBasic;
    [SerializeField] GameObject ZombieArm;
    [SerializeField] GameObject ZombieRibcage;
    [SerializeField] GameObject ZombieChubby;*/

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    void Update()
    {
        CheckZombies();
    }

    IEnumerator SpawnZombies()
    {
        spawnCount = totalSpawnCount;
        for (int i = 0; i < totalSpawnCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns); // Optional delay between spawns
            Vector3 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject randomZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
                Instantiate(randomZombiePrefab, spawnPosition, Quaternion.identity);
                spawnCount--;
            }

        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            spawnPosition = GetRandomNavMeshLocation();
            attempts++;
        } while (spawnPosition != Vector3.zero && Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer && attempts < 30);

        if (attempts >= 30)
        {
            return Vector3.zero; // If a valid position isn't found in 30 attempts, return Vector3.zero
        }
        return spawnPosition;
    }

    Vector3 GetRandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += player.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

    void CheckZombies()
    {
        if (spawnCount == 0 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Invoke("NextLevel", 5f);
        }
    }

    private void NextLevel()
    {
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }


/*    private void setDefaultValues()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        float multiplier = 1 + 0.35f * (level - 1); // Assuming a 35% increase per level

        // Basic Zombie
        ZombieBasic.GetComponent<NavMeshAgent>().speed = 5f * multiplier;
        ZombieBasic.GetComponent<ZombieStats>().health = (int)(100 * multiplier);
        ZombieBasic.GetComponent<ZombieStats>().damage = (int)(10 * multiplier);

        // Arm Zombie
        ZombieArm.GetComponent<NavMeshAgent>().speed = 6f * multiplier;
        ZombieArm.GetComponent<ZombieStats>().health = (int)(250 * multiplier);
        ZombieArm.GetComponent<ZombieStats>().damage = (int)(15 * multiplier);

        // Ribcage Zombie
        ZombieRibcage.GetComponent<NavMeshAgent>().speed = 10f * multiplier;
        ZombieRibcage.GetComponent<ZombieStats>().health = (int)(50 * multiplier);
        ZombieRibcage.GetComponent<ZombieStats>().damage = (int)(10 * multiplier);

        // Chubby Zombie
        ZombieChubby.GetComponent<NavMeshAgent>().speed = 7f * multiplier;
        ZombieChubby.GetComponent<ZombieStats>().health = (int)(200 * multiplier);
        ZombieChubby.GetComponent<ZombieStats>().damage = (int)(20 * multiplier);
    }*/
}

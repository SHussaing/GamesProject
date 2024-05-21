using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs; // Array of zombie prefabs to spawn
    public Transform player; // Assign the player transform in the inspector
    public float spawnRadius; // Radius within which zombies will spawn
    public int zombieCount; // Number of zombies to spawn

    private float noZombiesTime = 10f;
    private float timeToNextLevel = 5f;
    private bool noZombies = false;

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
        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 spawnPosition = GetRandomNavMeshLocation();
            if (spawnPosition != Vector3.zero)
            {
                GameObject randomZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
                Instantiate(randomZombiePrefab, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(10f); // Optional delay between spawns
        }
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
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            if (!noZombies)
            {
                noZombies = true;
                noZombiesTime = Time.time;
            }
            else if (Time.time - noZombiesTime >= timeToNextLevel)
            {
                NextLevel();
            }
        }
        else
        {
            noZombies = false;
        }
    }

    private void NextLevel()
    {
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}

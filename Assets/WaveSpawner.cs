using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject wallEnemyPrefab;
    public GameObject miniBoss1Prefab;
    public GameObject realBossPrefab;
    public GameObject hydraPrefab;

    public int miniBossCount;   // Increase this for each wave
    public int wallEnemyCount;  // Increase this for each wave
    public GameObject bossPrefab; // Switch between the main boss and the Hydra

    // Store spawn parameters like spawnRate, minZ, maxZ, etc. if they vary per wave
    public float spawnRate = 1f;  // Time interval between spawns
    public float minZ, maxZ;      // Spawn position boundaries
    public float spawnHeight = 1f; // Spawn height

    private EnemyManager manager;
    private int totalEnemyCount;
    private int remainingEnemyCount;

    private void Start()
    {
        manager = FindObjectOfType<EnemyManager>();
        totalEnemyCount = manager.GetTotalEnemyCount();
        remainingEnemyCount = manager.GetRemainingEnemyCount();
    }
    private void Update()
    {
        // You can fetch updates for these counts in the update loop if needed
        totalEnemyCount = manager.GetTotalEnemyCount();
        remainingEnemyCount = manager.GetRemainingEnemyCount();
    }
    public IEnumerator SpawnWave()
    {
        // Spawn miniBossCount minibosses at a regular interval
        for (int i = 0; i < miniBossCount; i++)
        {
            SpawnEnemy(miniBoss1Prefab);
            yield return new WaitForSeconds(spawnRate);
        }

        // Spawn wallEnemyCount WallEnemies at a regular interval
        for (int i = 0; i < wallEnemyCount; i++)
        {
            SpawnEnemy(wallEnemyPrefab);
            yield return new WaitForSeconds(spawnRate);
        }

        // Spawn the boss
        SpawnEnemy(bossPrefab);

        // Once everything has been spawned, wait for everything in this wave to be defeated
        // This is handled in Update method
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPosition = new Vector3(-50, spawnHeight, Random.Range(minZ, maxZ)); // Adjust as needed
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;  // Assuming all enemy prefabs have a script called "Enemy"
    }

    private void HandleEnemyDestroyed()
    {
        remainingEnemyCount--;
        if (remainingEnemyCount <= 0)
        {
            manager.OnWaveDefeated();
        }
    }
}

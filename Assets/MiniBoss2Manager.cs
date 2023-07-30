using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss2Manager : MonoBehaviour
{
    public GameObject miniBossPrefab;

    public float spawnRate;
    public float minZ, maxZ;
    public float spawnHeight;

    private float lastSpawnTime;

    private void Update()
    {

       // if (EnemyManager.isHydraActive) return;

        if (Time.time - lastSpawnTime > spawnRate)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemy()
    {
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 spawnPosition = new Vector3(50, spawnHeight, randomZ);

        GameObject spawnedEnemy = Instantiate(miniBossPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemy.GetComponent<MiniBoss02>().OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void HandleEnemyDestroyed()
    {
        // Do any post-destruction handling here, e.g. reducing the number of spawned minibosses
    }
}

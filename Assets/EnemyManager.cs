using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public HudManager hudManager;

    public Wave[] waves;

    public GameObject wallEnemyPrefab; // Wall - DONE
    public GameObject bossPrefab; // HydraBoss - DONE 
    public GameObject dronePrefab; // Drone - DONE
    public GameObject sniperPrefab; // Sniper - DONE
    public GameObject shielderPrefab; // Shielder - DONE
    public GameObject summonerPrefab; // Summoner
    public GameObject trapperPrefab; // Trapper - DONE
    public GameObject helperPrefab; // Helper - DONE
    public GameObject carrierPrefab; // Carrier - DONE
    
    public float spawnRate;
    public float minZ, maxZ;
    public float spawnHeight;

    private float lastSpawnTime;
    //private float spawnBossTimer = 30f; // LINE 52 in update has this already set for some reason. YOLO.
   
    private float timeWithoutEnemies = 0f;

    private int waveNumber = 1;


    public int wallEnemyCount;
    public int droneCount;

    private int totalEnemyCount;
    private int remainingEnemyCount;

    private List<float> wallLocationsX = new List<float>();
    private List<float> droneLocationsX = new List<float>();
    private const int MAX_WALL_ENEMIES = 10;  // Adjust this to whatever number you want
    private const int MAX_DRONE_ENEMIES = 5;  // Adjust this to whatever number you want

    private float minDroneDistance = 1f; // Minimum distance between drones

    public GameObject smokeEffectDronePrefab;  // add reference to your smoke effect prefab

    private void Start()
    {
        Wave currentWave = waves[waveNumber - 1];
        totalEnemyCount = CalculateTotalEnemyCountForWave(waveNumber - 1); // passing the index of current wave
        Debug.Log("TotalEnemiesSpawned: " + totalEnemyCount);
        remainingEnemyCount = totalEnemyCount;

        // Start the first wave
        StartCoroutine(SpawnWave());
    }
    private void Update()
    {
        // Check if there are no enemies
        if (remainingEnemyCount <= 0)
        {
            timeWithoutEnemies += Time.deltaTime;
            if (timeWithoutEnemies >= 5f) // Check if 5 seconds have passed
            {
                OnWaveDefeated();
                timeWithoutEnemies = 0f; // Reset the counter
            }
        }
        else
        {
            timeWithoutEnemies = 0f; // Reset the counter
        }
    }
    public IEnumerator SpawnWave()
    {

        Wave wave = waves[waveNumber - 1]; // assuming waveNumber starts from 1

        List<GameObject> enemySpawnOrder = new List<GameObject>();

        // Populate the enemySpawnOrder list based on the count of each enemy type in the wave
        for (int i = 0; i < wave.droneCount; i++) enemySpawnOrder.Add(dronePrefab);
        for (int i = 0; i < wave.wallCount; i++) enemySpawnOrder.Add(wallEnemyPrefab);
        for (int i = 0; i < wave.sniperCount; i++) enemySpawnOrder.Add(sniperPrefab);
        for (int i = 0; i < wave.shielderCount; i++) enemySpawnOrder.Add(shielderPrefab);
        for (int i = 0; i < wave.carrierCount; i++) enemySpawnOrder.Add(carrierPrefab);
        for (int i = 0; i < wave.helperCount; i++) enemySpawnOrder.Add(helperPrefab);
        for (int i = 0; i < wave.trapperCount; i++) enemySpawnOrder.Add(trapperPrefab);
        for (int i = 0; i < wave.summonerCount; i++) enemySpawnOrder.Add(summonerPrefab);
        for (int i = 0; i < wave.hydraBossCount; i++) enemySpawnOrder.Add(bossPrefab);

        // Shuffle the enemySpawnOrder list
        for (int i = 0; i < enemySpawnOrder.Count; i++)
        {
            GameObject temp = enemySpawnOrder[i];
            int randomIndex = Random.Range(i, enemySpawnOrder.Count);
            enemySpawnOrder[i] = enemySpawnOrder[randomIndex];
            enemySpawnOrder[randomIndex] = temp;
        }
        // Ensure shielder is not the first enemy
        if (enemySpawnOrder[0] == shielderPrefab && enemySpawnOrder.Count > 1)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(1, enemySpawnOrder.Count); // Avoid picking index 0
            } while (enemySpawnOrder[randomIndex] == shielderPrefab); // Ensure the selected enemy is not another shielder

            // Switch the shielder with another random enemy
            GameObject temp = enemySpawnOrder[0];
            enemySpawnOrder[0] = enemySpawnOrder[randomIndex];
            enemySpawnOrder[randomIndex] = temp;
        }
        // Spawn the enemies based on the shuffled order
        foreach (GameObject enemyPrefab in enemySpawnOrder)
        {
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(spawnRate);
        }
    }
    private void SpawnEnemy(GameObject prefab)
    {
        float x;
        float spawnHeightForPrefab = spawnHeight; // use a local variable that we can modify per prefab
        float spawnZForPrefab = Random.Range(minZ, maxZ); // use a local variable that we can modify per prefab

        if (prefab == wallEnemyPrefab && wallLocationsX.Count >= MAX_WALL_ENEMIES) // Check if it's a wall enemy
        {
            // Don't spawn if we already have the maximum number of wall enemies
            return;
        }
        else if (prefab == dronePrefab && droneLocationsX.Count >= MAX_DRONE_ENEMIES) // Check if it's a drone
        {
            // Don't spawn if we already have the maximum number of drone enemies
            return;
        }

        if (prefab == wallEnemyPrefab) // Check if it's a wall enemy
        {
            do
            {
                x = Random.Range(-7f, 6f);
            } while (wallLocationsX.Contains(x));

            wallLocationsX.Add(x);
            spawnZForPrefab = 0f;
        }
        else if (prefab == dronePrefab) // Check if it's a drone
        {
            do
            {
                x = Random.Range(-7f, 7f);
            } while (droneLocationsX.Exists(droneX => Mathf.Abs(droneX - x) < minDroneDistance)); // Avoid spawning too close to an existing drone

            droneLocationsX.Add(x);
            spawnHeightForPrefab = 1f; // Drones always spawn at a height of 1
            spawnZForPrefab = 0.1f; // Drones always spawn at a z of 0.1
        }
        else if (prefab == sniperPrefab) // Check if it's a sniper
        {
            x = (Random.value > 0.5f) ? -50f : 50f;
            spawnHeightForPrefab = 1f;
            spawnZForPrefab = 5f;
        }
        else
        {
            x = 0; // Default x location for other enemies
        }

        Vector3 spawnPosition = new Vector3(x, spawnHeightForPrefab, spawnZForPrefab);
        if (prefab == dronePrefab)
        {
            Instantiate(smokeEffectDronePrefab, spawnPosition, Quaternion.identity);
        }
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        if (prefab == wallEnemyPrefab)
        {
            enemy.GetComponent<wallEnemyController>().SetWaveNumber(waveNumber);
        }

        enemy.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void HandleEnemyDestroyed()
    {
        Debug.Log("Enemy destroyed.");
        remainingEnemyCount--;

        Debug.Log("Remaining enemy count: " + remainingEnemyCount);
        if (remainingEnemyCount <= 0)
        {
            var enemiesInScene = FindObjectsOfType<Enemy>();
           
            if (enemiesInScene.Length == 0)
            {
                Debug.Log("All enemies are defeated. Moving to the next wave.");
                OnWaveDefeated();
            }
        }
    }

    public void OnWaveDefeated()
    {
        waveNumber++;
        hudManager.UpdateWaveText(waveNumber);

        // Don't increase enemy counts here

        totalEnemyCount = CalculateTotalEnemyCountForWave(waveNumber - 1);
        remainingEnemyCount = totalEnemyCount;

        // Clear locations of all enemies
        wallLocationsX.Clear();
        droneLocationsX.Clear();
        // ... repeat for each enemy type

        StartCoroutine(SpawnWave());
    }

    private int CalculateTotalEnemyCountForWave(int waveNumber)
    {
        Wave wave = waves[waveNumber];
        return wave.droneCount + wave.wallCount + wave.sniperCount + wave.shielderCount + wave.summonerCount + wave.trapperCount + wave.helperCount + wave.carrierCount + wave.hydraBossCount;
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        Debug.Log("Enemy destroyed.");
        enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        remainingEnemyCount--;

        Debug.Log("Remaining enemy count: " + remainingEnemyCount);

        if (remainingEnemyCount <= 0)
        {
            var enemiesInScene = FindObjectsOfType<Enemy>();
            if (enemiesInScene.Length == 0)
            {
                Debug.Log("All enemies are defeated. Moving to the next wave.");
                OnWaveDefeated();
            }
        }
    }
   
   
    public void DestroyEnemies()
    {
        // Find all enemies and bosses in the scene and unsubscribe from their events
        var enemiesInScene = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemiesInScene)
        {
            enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        }
    }

    private void OnDestroy()
    {
        DestroyEnemies();
    }

    public int GetTotalEnemyCount()
    {
        return totalEnemyCount;
    }

    public int GetRemainingEnemyCount()
    {
        return remainingEnemyCount;
    }

}

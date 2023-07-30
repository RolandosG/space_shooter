using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnCooldown = 5.0f;
    public float minSpawnX = -7.0f;
    public float maxSpawnX = 7.0f;
    public float obstacleSpeed = 30.0f;

    public GameObject spawnAnimationPrefab;
    public float animationDuration = 3.0f;

    private float timeSinceLastSpawn;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnCooldown)
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0;
        }

    }

    void SpawnObstacle()
    {
        // Calculate a random X position between minSpawnX and maxSpawnX
        float randomX = Random.Range(minSpawnX, maxSpawnX);

        // Calculate the spawn position (in front of the Hydra)
        Vector3 spawnPosition = new Vector3(Random.Range(-7.0f, 7.0f), 2.0f, -30.0f);

        // Call the SpawnAnimation method
        StartCoroutine(SpawnAnimation(spawnPosition));
    }
    IEnumerator SpawnAnimation(Vector3 spawnPosition)
    {
        // Instantiate the spawn animation prefab
        GameObject spawnAnimation = Instantiate(spawnAnimationPrefab, spawnPosition, Quaternion.identity);

        // Wait for the animation duration
        yield return new WaitForSeconds(animationDuration);

        // Destroy the spawn animation
        Destroy(spawnAnimation);

        // Instantiate the obstacle
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // Set the obstacle to move towards the player
        ObstacleController obstacleController = obstacle.GetComponent<ObstacleController>();
        obstacleController.direction = Vector3.back;
        obstacleController.speed = obstacleSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Enemy
{
    // Death animation
    public GameObject explosionPrefab;
    // Death animation
    public GameObject LargeExplosionPrefab;
    // Reference to the orb prefab
    public GameObject orbPrefab;

    private PlayerController m_Pc;

    // References to the spawn points
    private Transform[] topSpawnPoints;
    private Transform[] middleSpawnPoints;
    private Transform[] bottomSpawnPoints;

    public Material defaultMaterial;  // Default material of the spawn points
    public Material firingMaterial;  // Material used when a spawn point is about to fire

    public float speed = 1f;  // Speed at which the carrier moves
    private int health ; // current health here
    public int baseHealth = 100;  // Base health of the sniper
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public List<PatternStep> patternSteps; // List of all pattern steps

    void Start()
    {
        health = baseHealth;  // Initialize current health with base health
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }
        // Get references to the spawn points
        topSpawnPoints = GetSpawnPoints("Top");
        middleSpawnPoints = GetSpawnPoints("Middle");
        bottomSpawnPoints = GetSpawnPoints("Bottom");

        // Set the initial and target positions
        int initialX = Random.Range(0, 2) == 0 ? -50 : 50;
        startPosition = new Vector3(initialX, 1, 10);
        targetPosition = new Vector3(0, 1, 10);
        transform.position = startPosition;

        // Start the movement coroutine
        StartCoroutine(MoveToTargetPosition());
    }

    private Transform[] GetSpawnPoints(string name)
    {
        Transform container = transform.Find(name);
        Transform[] spawnPoints = new Transform[container.childCount];
        for (int i = 0; i < container.childCount; i++)
        {
            spawnPoints[i] = container.GetChild(i);
            // Set the default material of each spawn point
            spawnPoints[i].GetComponent<MeshRenderer>().material = defaultMaterial;
        }
        return spawnPoints;
    }

    private IEnumerator SpawnOrbs()
    {
        while (true)
        {
            // Create a new random pattern
            CreateRandomPattern();
            PatternStep currentStep = patternSteps[0];

            foreach (Transform spawnPoint in currentStep.spawnPoints)
            {
                // Check if the spawn point is blocked by an enemy
                if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out RaycastHit hit))
                {
                    // If the raycast hit an object with an Enemy component, skip this spawn point
                    if (hit.transform.GetComponent<Enemy>() != null)
                    {
                        Debug.DrawLine(spawnPoint.position, hit.point, Color.red, 2f);  // Draw a red line for 2 seconds
                        continue;
                    }
                }

                StartCoroutine(SpawnOrb(spawnPoint));
            }

            // Wait for a bit before the next spawn
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator MoveToTargetPosition()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Create the firing pattern
        CreateRandomPattern();

        // Start the spawning behavior
        StartCoroutine(SpawnOrbs());
    }

    private IEnumerator SpawnOrb(Transform spawnPoint)
    {
        // Change the material to indicate that the spawn point is about to fire
        spawnPoint.GetComponent<MeshRenderer>().material = firingMaterial;

        // Send a raycast forward from the spawn point
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward * 2.5f, out RaycastHit hit))
        {
            // If the raycast hit an object with an Enemy component, stop this coroutine
            if (hit.transform.GetComponent<Enemy>() != null)
            {
                Debug.DrawLine(spawnPoint.position, hit.point, Color.red, 2f);  // Draw a red line for 2 seconds
                Instantiate(orbPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnPoint.GetComponent<MeshRenderer>().material = defaultMaterial;
                yield break;
            }
        }

        yield return new WaitForSeconds(0.5f);  // Wait for a bit

        // Instantiate the orb and change the material back to the default
        Instantiate(orbPrefab, spawnPoint.position, Quaternion.identity);
        spawnPoint.GetComponent<MeshRenderer>().material = defaultMaterial;
    }


    private void CreateRandomPattern()
    {
        patternSteps = new List<PatternStep>();  // Clear the list to start a new pattern

        // Create a list with all the spawn points
        List<Transform> allSpawnPoints = new List<Transform>(topSpawnPoints);
        allSpawnPoints.AddRange(middleSpawnPoints);
        allSpawnPoints.AddRange(bottomSpawnPoints);

        // Shuffle the list
        System.Random rng = new System.Random();
        int n = allSpawnPoints.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Transform value = allSpawnPoints[k];
            allSpawnPoints[k] = allSpawnPoints[n];
            allSpawnPoints[n] = value;
        }

        // Use the first 8 spawn points for the pattern
        PatternStep step = new PatternStep();
        for (int i = 0; i < 8; i++)
        {
            step.spawnPoints.Add(allSpawnPoints[i]);
        }
        patternSteps.Add(step);
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;

        if (health <= 0)
        {
            Defeat();
        }
    }

    public override void Defeat()
    {
        // Start shaking the carrier
        StartCoroutine(Shake(5f, 0.4f));

        // Spawn random explosions
        StartCoroutine(SpawnExplosions(5f, 10));

        // Delay the final destruction and scoring by 3 seconds
        Invoke(nameof(FinalizeDefeat), 5f);
       
    }
    private IEnumerator SpawnExplosions(float duration, int explosionCount)
    {
        for (int i = 0; i < explosionCount; i++)
        {
            // Delay between explosions
            yield return new WaitForSeconds(duration / explosionCount);

            // Get a random point on the carrier
            Vector3 explosionPoint = transform.position + new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );

            // Spawn the explosion
            Instantiate(explosionPrefab, explosionPoint, Quaternion.identity);
        }
    }
    private void FinalizeDefeat()
    {
        if (m_Pc != null)
        {
            m_Pc.RecieveScore();
        }

        Instantiate(LargeExplosionPrefab, transform.position, Quaternion.identity);
        base.Defeat();
    }
    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}

[System.Serializable]
public class PatternStep
{
    public List<Transform> spawnPoints = new List<Transform>();
}

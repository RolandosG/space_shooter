using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss01 : MonoBehaviour
{
   

    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnEnemyDestroyed;
    
    public GameObject explosionPrefab;
    public GameObject projectilePrefab;

    public int health;
   
    public Transform[] gunPositions;
    public float projectileSpeed;
    public float fireRate;
    public int burstCount;
    public float burstDelay;

    private float spawnY; // Spawn position in y axis

    private PlayerController m_Pc;

    private int baseHealth; // this is your base health
    private float speed = 16.0f; // Declare as a member variable
    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
       
        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }


        float spawnX = Random.value > 0.5 ? -50f : 50f; // Spawn at either -50 or 50 on the x-axis
        spawnY = Random.Range(-7f, 7f); // Random position in y axis
        transform.position = new Vector3(spawnX, spawnY, 0f);
        StartCoroutine(StartBehaviors());

        // initialize baseHealth
        baseHealth = health;
    }

    void Update()
    {
        // Assuming the movement should be between -7 and 7 in x direction.
        float newXPosition = transform.position.x + speed * Time.deltaTime;

        // If the new x position is beyond the boundaries, change the direction of the speed.
        if (newXPosition < -7 || newXPosition > 7)
        {
            speed *= -1;

            // Recalculate the new X position with the reversed speed
            newXPosition = transform.position.x + speed * Time.deltaTime;
        }

        // Apply the new position.
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

    }


    public IEnumerator StartBehaviors()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2);
        StartCoroutine(MoveAndShoot());
    }

    public IEnumerator MoveAndShoot()
    {
        while (true)
        {
            // Move to random location within boundaries
            Vector3 targetPosition = GetRandomPositionWithinBoundary();
            yield return StartCoroutine(MoveToTargetPosition(targetPosition));

            // Fire a burst of projectiles
            for (int i = 0; i < burstCount; i++)
            {
                FireProjectiles();
                yield return new WaitForSeconds(fireRate);
            }
        }
    }

    Vector3 GetRandomPositionWithinBoundary()
    {
        float randomX = Random.Range(-7f, 7f);
        return new Vector3(randomX, spawnY, 0f);
    }

    IEnumerator MoveToTargetPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            // Rotate according to movement here
            yield return null;
        }
    }

    void FireProjectiles()
    {
        foreach (Transform gunPosition in gunPositions)
        {
            GameObject projectile = Instantiate(projectilePrefab, gunPosition.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
        }
    }
    public void SetWaveNumber(int waveNumber)
    {
        // This will increase the health according to the wave number.
        // Modify this according to your needs.
        health = baseHealth * waveNumber;
    }

    public void TakeDamage(int damage)
        {
        health -= damage;
            if (health <= 0)
            {
                if (OnEnemyDestroyed != null)
                {
                    OnEnemyDestroyed();
                }
                Explode();

            }
        }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Explode();
            
            // Add destruction animation or particle effect here
        }
    }
    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (m_Pc != null) // add null check here
        {
            m_Pc.RecieveScore();
        }
        Destroy(gameObject);
    }
 
}

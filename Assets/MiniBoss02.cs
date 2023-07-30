using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss02 : MonoBehaviour
{
    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnEnemyDestroyed;
   
    public GameObject explosionPrefab;
    public GameObject projectilePrefab;
    
    private PlayerController m_Pc;
    public int health;

    [SerializeField]
    private float approachSpeed = 12f;
    [SerializeField]
    private float shootingInterval = 2f; // Time between each shot, in seconds
   
    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }
        StartCoroutine(MoveToCenterThenRandomX());
        StartCoroutine(ShootProjectiles());
    }

    private IEnumerator MoveToCenterThenRandomX()
    {
        Vector3 centerPosition = new Vector3(0, transform.position.y, transform.position.z);

        while (Vector3.Distance(transform.position, centerPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, centerPosition, approachSpeed * Time.deltaTime);
            yield return null;
        }

        // Move to a random position on the x-axis between -7 and 7
        Vector3 randomXPosition = new Vector3(Random.Range(-7f, 7f), transform.position.y, transform.position.z);

        while (Vector3.Distance(transform.position, randomXPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, randomXPosition, approachSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator ShootProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootingInterval);
            ShootProjectile();
        }
    }
    private void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // Add logic to control the projectile's direction and speed, if needed
        }
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

        // Add a collider to prevent further interactions with the enemy object
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Destroy the enemy object after a delay
        float destroyDelay = 0.3f; // Adjust this value according to the duration of the explosion animation or particle effect
        Destroy(gameObject, destroyDelay);
    }
}

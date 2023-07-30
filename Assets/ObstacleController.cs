using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Vector3 direction;
    public int health;
    public float speed;

    public GameObject explosionPrefab;

    private int currentHealth;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = health;
    }

    void Update()
    {
        rb.velocity = direction * speed;
        transform.position += direction * speed * Time.deltaTime;

        if (transform.position.z <= -100.0f)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health--;

            // Destroy the player's projectile
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                // Destroy the obstacle
                Explode();
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(20);
            }
            // Optionally, you can destroy the bullet after dealing damage
            Explode();
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Explode();
        }
    }
    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

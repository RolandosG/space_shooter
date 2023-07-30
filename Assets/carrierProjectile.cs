using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carrierProjectile : MonoBehaviour
{
    public float speed = 25f; // Speed of the projectile
    public float lifetime = 2f; // Time after which the projectile is destroyed
    public int damage = 10; // Damage dealt by the projectile

    public GameObject explosionPrefab;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime has expired
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime); // Move the projectile forward
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(10);
                Explode();
            }

            // Optionally, you can destroy the bullet after dealing damage
            Explode();
        }
    }
    
}

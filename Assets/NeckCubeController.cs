using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckCubeController : MonoBehaviour
{
    public int health = 10; // Set the health of the cube
    public GameObject explosionPrefab;

    public void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage; // Reduce health by the damage amount

        if (health <= 0) // Check if health is 0 or less
        {
            // Instantiate the explosion at the position of the wall and with the same rotation
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject); // Destroy the cube
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public int health = 10; // Set the health of the cube
    public float rotationSpeed = 50f; // Set the rotation speed of the cube
    public GameObject explosionPrefab; // Drag your explosion prefab here
    void Update()
    {
        // Rotate the cube around its Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
    // Method to handle the damage taken by the cube
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

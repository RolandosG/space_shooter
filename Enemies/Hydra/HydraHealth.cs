using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraHealth : MonoBehaviour
{
    public int health = 10; // Set the health of the cube
    private BodyManager bodyManager;
    private bool isDestroyed = false;

    private void Start()
    {
        // Get reference to BodyManager in the scene
        bodyManager = FindObjectOfType<BodyManager>();

        if (bodyManager == null)
        {
            Debug.LogError("BodyManager not found in the scene.");
        }
    }

    // Method to handle the damage taken by the cube
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && !isDestroyed)
        {
            if (bodyManager != null)
            {
                StartCoroutine(bodyManager.Die());
            }

            isDestroyed = true;
        }
    }

}

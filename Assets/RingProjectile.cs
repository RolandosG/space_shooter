using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingProjectile : MonoBehaviour
{
    public float growthRate = 9.0f;
    public float maxScale = 20.0f;
    public float scaleSpeed = 2.1f;
    public float projectileSpeed = 10f;
    public int projectileCount = 10;
    public GameObject projectilePrefab;

    private float currentScale = 0f;
    private bool hasFired = false;

    private void Update()
    {
        // Increase the radius of the ring
        float growth = growthRate * Time.deltaTime;
        transform.localScale += new Vector3(growth, 0f, growth);

        // Scale the projectiles to compensate for the increased radius
        float projectileScale = transform.localScale.x / currentScale;
        currentScale = transform.localScale.x;

        // Check if the ring has finished scaling up and hasn't fired yet
        if (currentScale >= 1f && !hasFired)
        {
            hasFired = true;
            FireProjectiles();
        }
        if (currentScale >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    private void FireProjectiles()
    {
        // Calculate the angle between each projectile
        float angleBetween = 360f / projectileCount;

        // Spawn the projectiles in a circular pattern around the ring
        for (int i = 0; i < projectileCount; i++)
        {
            // Calculate the angle for this projectile
            float angle = i * angleBetween;

            // Calculate the position for this projectile
            Vector3 position = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad)) * (currentScale * 0.5f);

            // Spawn the projectile at the calculated position and rotation
            GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);

            // Scale the projectile to compensate for the increased radius
            projectile.transform.localScale *= currentScale;

            // Set the velocity of the projectile to be in the direction of the center of the ring
            Vector3 direction = (transform.position - position).normalized;
            projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.RecieveDamage();
            }
            Destroy(gameObject);
        }
    }
}

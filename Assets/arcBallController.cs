using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcBallController : MonoBehaviour
{
    // for the drone to know when to move
    public delegate void OnProjectileDestroyDelegate();
    public static event OnProjectileDestroyDelegate OnProjectileDestroyEvent;

    public Vector3 targetPosition;

    public GameObject explosionPrefab;
    public GameObject landingIndicatorPrefab;  // This is the prefab for the landing indicator
    private GameObject landingIndicatorInstance;  // Instance of the landing indicator prefab

    public float speed = 5f;
    public float upwardForce = 5f;  // Increase this value for a higher arc
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate upward force
        //float upwardForce = ((targetPosition.y - transform.position.y) / direction.y) * Physics.gravity.y;

        // Set the velocity
        rb.velocity = direction * speed + Vector3.up * upwardForce;

        // Calculate hit point:
        float timeToHitGround;
        if ((2 * (transform.position.y + upwardForce)) / Mathf.Abs(Physics.gravity.y) >= 0)
        {
            timeToHitGround = Mathf.Sqrt((2 * (transform.position.y + upwardForce)) / Mathf.Abs(Physics.gravity.y));  // Time for the projectile to hit the ground
        }
        else
        {
            Debug.LogError("Invalid calculation for time to hit ground.");
            return;
        }
        Vector3 hitPoint = new Vector3(targetPosition.x, 0, targetPosition.z);

        hitPoint.y = 0;  // Assuming ground is at y = 0

        // Instantiate the landing indicator at the hit point
        landingIndicatorInstance = Instantiate(landingIndicatorPrefab, hitPoint, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the projectile to face the direction it's moving
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        // If the projectile falls below y=-1, destroy it
        if (transform.position.y <= -1)
        {

            Explode();
        }
        // Remove landing indicator when the projectile hits the ground (or goes below y=0)
        if (transform.position.y <= 0 && landingIndicatorInstance != null)
        {
            Destroy(landingIndicatorInstance);
        }

    }
    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (OnProjectileDestroyEvent != null)
        {
            OnProjectileDestroyEvent.Invoke();
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(20);
            }

            // Optionally, you can destroy the fireball after dealing damage
            Explode();
        }
    }
}

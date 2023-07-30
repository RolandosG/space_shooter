using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossProjectile : MonoBehaviour
{
    public float despawnTime = 3f;
    public float minVelocity = 0.1f;
    public Vector3 minBounds = new Vector3(-10, -10, -10);
    public Vector3 maxBounds = new Vector3(10, 10, 10);

    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Velocity check
        if (rb.velocity.magnitude < minVelocity)
        {
            Destroy(gameObject);
        }

        // Boundary check
        if (transform.position.x < minBounds.x || transform.position.x > maxBounds.x ||
            transform.position.y < minBounds.y || transform.position.y > maxBounds.y ||
            transform.position.z < minBounds.z || transform.position.z > maxBounds.z)
        {
            Destroy(gameObject);
        }

        // Time limit
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0f)
        {
            Destroy(gameObject);
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

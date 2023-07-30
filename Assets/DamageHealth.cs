using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHealth : MonoBehaviour
{
    public float speed = 25f;  // Speed of the damage debuff
    public float rotationSpeed = 50f;  // Rotation speed
    private Vector3 targetPosition;

    private PlayerController m_Pc;


    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }

        // Set the target position to be just below the current position
        targetPosition = new Vector3(transform.position.x, 1, transform.position.z);

    }

    void Update()
    {
        // Rotate the damage debuff around its local Y axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // If the damage debuff is not at the target position yet, move towards it
        if (transform.position.y > targetPosition.y)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        // Otherwise, start moving it towards negative Z
        else
        {
            transform.position -= Vector3.forward * speed * Time.deltaTime;
        }

        // If the damage debuff has gone past -50 in the Z direction, destroy it
        if (transform.position.z < -100)
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
                m_Pc.TakeDamage(10); // The player takes damage when touching the debuff
            }

            // Optionally, you can destroy the damage debuff after it has been picked up
            Destroy(gameObject);
        }
    }
}
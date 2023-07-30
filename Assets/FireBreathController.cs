using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathController : MonoBehaviour
{
    public Transform hydraHead;
    public float breathSpeed = 5.0f;
    public float zigzagSpeed = 2.0f;
    public float zigzagAmplitude = 75.0f;
    public float smoothTransitionRange = 36.0f;

    private Transform playerTransform;
    private float initialXRotation = 96.0f;
    private float targetYRotation;
    private bool targetYRotationSet = false;

    public ParticleSystem fireBreathParticleSystem; // Reference to the fire breath particle system

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        fireBreathParticleSystem = GetComponentInChildren<ParticleSystem>(); // Get the particle system in the child object
    }

    void Update()
    {
        // Calculate the direction vector from the Hydra head to the player
        Vector3 direction = (playerTransform.position - hydraHead.position).normalized;

        // Calculate the X-axis rotation based on the breathSpeed
        initialXRotation -= (breathSpeed * Time.deltaTime);
        float xRotation = Mathf.Clamp(initialXRotation, 0.0f, 66.0f);

        // Calculate the Y-axis rotation based on the zigzag motion
        float yRotation;
        if (xRotation <= 150.0f)
        {
            if (!targetYRotationSet)
            {
                targetYRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                targetYRotationSet = true;
            }
            yRotation = targetYRotation;
        }
        else
        {
            float zigzagRotation = 180.0f + zigzagAmplitude * Mathf.Sin(Time.time * zigzagSpeed);
            if (xRotation < 204.0f)
            {
                float t = (xRotation - 150.0f) / smoothTransitionRange;
                yRotation = Mathf.Lerp(targetYRotation, zigzagRotation, t);
            }
            else
            {
                yRotation = zigzagRotation;
            }
        }

        // Set the new rotation
        transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

        // Emit particles for fire breath effect
        if (!fireBreathParticleSystem.isPlaying && xRotation <= 30.0f)
        {
            fireBreathParticleSystem.Play();
        }
    }
}

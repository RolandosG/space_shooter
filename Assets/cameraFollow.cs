using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target; // Assign the player's transform to this in the inspector.
    public float smoothSpeed = 0.125f; // Adjust this to make the camera follow more or less smoothly.
    public Vector3 offset; // This allows you to offset the position of the camera relative to the player.

    private void FixedUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}

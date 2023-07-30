using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    public float hoverSpeed = 1.0f; // the speed of the hovering movement
    public float hoverHeight = 0.5f; // the maximum height the object will hover
    public float hoverRange = 0.5f; // the range of the hovering movement

    private Vector3 initialPosition; // the initial position of the object

    private void Start()
    {
        initialPosition = transform.position; // store the initial position of the object
    }

    private void Update()
    {
        // calculate the new position of the object based on time
        float newPositionY = initialPosition.y + (Mathf.Sin(Time.time * hoverSpeed) * hoverRange);
        Vector3 newPosition = new Vector3(initialPosition.x, newPositionY, initialPosition.z);

        // clamp the new position to the maximum hover height
        if (newPosition.y > initialPosition.y + hoverHeight)
        {
            newPosition.y = initialPosition.y + hoverHeight;
        }

        // set the object's position to the new position
        transform.position = newPosition;
    }
}

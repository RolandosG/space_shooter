using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapper : Enemy
{
    public float speed = 5f;  // Speed at which the trapper moves
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public GameObject trapPrefab;
    public float dropInterval = 5f; // How often the trapper drops traps (in seconds)

    void Start()
    {
        // Set the initial and target positions
        int initialX = Random.Range(0, 2) == 0 ? -50 : 50;
        int targetX = initialX == -50 ? 50 : -50;  // Set the target X to be the opposite of initial X
        int initialZ = Random.Range(-50, -10);

        float initialY = Random.Range(10f, 20f); // Set the y-axis value to be a random value between 20 and 30

        startPosition = new Vector3(initialX, initialY, initialZ);
        targetPosition = new Vector3(targetX, initialY, initialZ);
        transform.position = startPosition;

        // Start the movement coroutine
        StartCoroutine(MoveToTargetPosition());

        // Start the drop traps coroutine
        StartCoroutine(DropTrap());
    }


    private IEnumerator MoveToTargetPosition()
    {
        Debug.Log("Starting MoveToTargetPosition Coroutine");
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Debug.Log("Moving towards target: " + targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Reached Target Position");
        base.Defeat();
    }

    IEnumerator DropTrap()
    {
        while (true)
        {
            // Wait for a random drop interval between 3 and 6 seconds
            float dropInterval = Random.Range(3f, 6f);
            yield return new WaitForSeconds(dropInterval);

            // Instantiate the trap at the trapper's position, slightly offset downwards
            Vector3 dropPosition = transform.position + new Vector3(0, -1, 0);
            Instantiate(trapPrefab, dropPosition, Quaternion.identity);
        }
    }
}

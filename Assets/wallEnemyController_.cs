using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEnemyController_ : MonoBehaviour
{
    public GameObject wallEnemyPrefab;
    public int health;
    public float speed;
    public int numberOfCubes = 3; // Set this to the minimum number of cubes you want
    public float spacing = 2f;  // Add this line to the class variables

    private void Start()
    {
        // Spawn additional cubes
        for (int i = 1; i < numberOfCubes; i++)
        {
            // The position of the new cubes will be offset to the right of the original cube
            Vector3 newPosition = transform.position + new Vector3(i * spacing, 0, 0);
            Instantiate(wallEnemyPrefab, newPosition, Quaternion.identity);
        }

        // Start moving towards the player
        StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (transform.position.z > -50)  // Assuming the player is located at z=-50
        {
            transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
            yield return null;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

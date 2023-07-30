using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallEnemyController : Enemy
{
    public GameObject wallEnemyPrefab;
    public float baseHealth;
    private float health;
    public float speed;
    public int numberOfCubes = 3;
    public float spacing = 2f;
    public bool isOriginal = true;


    private void Start()
    {
        // Only spawn additional cubes if this is the original cube
        if (isOriginal)
        {
            for (int i = 1; i < numberOfCubes; i++)
            {
                Vector3 newPosition = transform.position + new Vector3(i * spacing, 0, 0);
                GameObject newCube = Instantiate(wallEnemyPrefab, newPosition, Quaternion.identity);
                newCube.GetComponent<wallEnemyController>().isOriginal = false;
            }
        }

        StartCoroutine(MoveTowardsPlayer());
        // initialize baseHealth
        baseHealth = health;
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (transform.position.z > -90)
        {
            transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
            yield return null;
        }

        Defeat();
    }
    public override void Defeat()
    {
        base.Defeat();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Defeat();
        }
    }

    public void SetWaveNumber(int waveNumber)
    {
        health = baseHealth * waveNumber;
    }
}

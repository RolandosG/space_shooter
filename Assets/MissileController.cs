using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    public GameObject explosionPrefab;

    public int damage = 10;
   
    private float m_ThreshHoldPositionZ = 85.0f;

    private Rigidbody rb;
    public float velocityThreshold = 0.1f;

    private AudioSource spawnSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        spawnSound = GetComponent<AudioSource>();

        if (spawnSound != null)
        {
            spawnSound.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EvaderEnemy"))
        {
            Explode();
        }
        else if (collision.gameObject.CompareTag("CatcherEnemy"))
        {
            Explode();
        }
        else if (collision.gameObject.CompareTag("MiniBoss01"))
        {
            collision.gameObject.GetComponent<MiniBoss01>().TakeDamage(damage);
            Explode();
        }
        else if (collision.gameObject.CompareTag("MiniBoss02"))
        {
            collision.gameObject.GetComponent<MiniBoss02>().TakeDamage(damage);
            Explode();
        }
        else if (collision.gameObject.CompareTag("RealBoss"))
        {
            collision.gameObject.GetComponent<RealBoss>().TakeDamage(damage);
            Explode();
        }
        else if (collision.gameObject.CompareTag("HydraHead")) // Add this condition
        {
            HydraHeadHealth headHealth = collision.gameObject.GetComponent<HydraHeadHealth>();
            if (headHealth != null)
            {
                headHealth.TakeDamage(damage); // You can adjust the damage value as needed
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("HydraCube"))
        {
            WallHealth cubeHealth = collision.gameObject.GetComponent<WallHealth>();
            if (cubeHealth != null)
            {
                cubeHealth.TakeDamage(damage); // Apply damage to the cube
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("HydraHeart"))
        {
            HydraHealth heartHealth = collision.gameObject.GetComponent<HydraHealth>();
            if (heartHealth != null)
            {
                heartHealth.TakeDamage(damage); // Apply damage to the cube
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("HydraObstacle"))
        {
            ObstacleController obstacleController = collision.gameObject.GetComponent<ObstacleController>();
            if (obstacleController != null)
            {
                obstacleController.TakeDamage(damage); // Apply damage to the obstacle
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("HydraNeckCube"))
        {
            NeckCubeController neckCubeController = collision.gameObject.transform.GetComponent<NeckCubeController>();
            if (neckCubeController != null)
            {
                neckCubeController.TakeDamage(damage);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("WallEnemy"))
        {
            wallEnemyController wallEnemyController = collision.gameObject.transform.GetComponent<wallEnemyController>();
            if (wallEnemyController != null)
            {
                wallEnemyController.TakeDamage(damage);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("Drone"))
        {
            Drone Drone = collision.gameObject.transform.GetComponent<Drone>();
            if (Drone != null)
            {
                Drone.TakeDamage(damage);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("Sniper"))
        {
            Sniper Sniper = collision.gameObject.transform.GetComponent<Sniper>();
            if (Sniper != null)
            {
                Sniper.TakeDamage(damage);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("Defender"))
        {
            Shielder Shielder = collision.gameObject.transform.GetComponent<Shielder>();
            if (Shielder != null)
            {
                Shielder.TakeDamage(5);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            Shield Shield = collision.gameObject.transform.GetComponent<Shield>();
            if (Shield != null)
            {
                Shield.TakeDamage(damage);
            }
            Explode();
        }
        /*else if (collision.gameObject.CompareTag("Defender"))
        {
            Shielder Shielder = collision.gameObject.transform.GetComponent<Shielder>();
            if (Shielder != null)
            {
                Shielder.TakeDamage(10);
            }
            Explode();
        }*/
        else if (collision.gameObject.CompareTag("Carrier"))
        {
            Carrier Carrier = collision.gameObject.transform.GetComponent<Carrier>();
            if (Carrier != null)
            {
                Carrier.TakeDamage(damage);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("Summoner"))
        {
            Summoner Summoner = collision.gameObject.transform.GetComponent<Summoner>();
            if (Summoner != null)
            {
                Summoner.TakeDamage(damage);
            }
            Explode();
        }
        else if (collision.gameObject.CompareTag("Splitter"))
        {
            Splitter Splitter = collision.gameObject.transform.GetComponent<Splitter>();
            if (Splitter != null)
            {
                Splitter.TakeDamage(damage);
            }
            Explode();
        }
    }
        private void Update()
        {
            if (transform.position.z >= m_ThreshHoldPositionZ)
            {
                Explode();
            }
            else if (rb.velocity.x < 0)
            {
                Explode();
            }
        // Check if the missile has any velocity in the x or y directions
        else if (Mathf.Abs(rb.velocity.x) > velocityThreshold || Mathf.Abs(rb.velocity.y) > velocityThreshold)
            {
                Explode();
            }
        }
    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

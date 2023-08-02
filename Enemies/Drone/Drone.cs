using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{

    private PlayerController m_Pc;

    public GameObject projectilePrefab;
    public GameObject explosionPrefab;
    public Transform projectileSpawnPoint;

    public int baseHealth;
    private int health;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }
        if (baseHealth == 0)
        {
            baseHealth = 20;
        }
        health = baseHealth;

        StartCoroutine(MoveAndShoot());
    }

    IEnumerator MoveAndShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        arcBallController arcBallController = projectile.GetComponent<arcBallController>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            arcBallController.targetPosition = playerPosition;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Defeat();
        }
    }
    public override void Defeat()
    {
        Debug.Log("Defeat method called in Drone class.");
        Explode();
    }
    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        if (m_Pc != null)
        {
            m_Pc.RecieveScore();
        }

        base.Defeat();
    }

    void OnEnable()
    {
        arcBallController.OnProjectileDestroyEvent += HandleProjectileDestroyEvent;
    }

    void OnDisable()
    {
        arcBallController.OnProjectileDestroyEvent -= HandleProjectileDestroyEvent;
    }

    void HandleProjectileDestroyEvent()
    {
        StartCoroutine(StartMoveAfterDelay(2f));
    }
    IEnumerator StartMoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    public void SetWaveNumber(int waveNumber)
    {
        health = baseHealth * waveNumber;
    }
}

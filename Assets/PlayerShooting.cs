using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public float projectileSpeed = 10f;
    public float baseFireRate = 0.5f;  // The base fire rate

    private float fireRate;  // The actual fire rate, this is the value that will be affected by the upgrade
    private float nextFire = 0.0f;
    private int fireRateUpgradeLevel;  // The current upgrade level of the fire rate
    public int damage = 10;  
    private void Start()
    {
        // Initialize the fire rate and the upgrade level
        fireRate = baseFireRate;
        fireRateUpgradeLevel = 0;  // Or load this value from your save system
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Vector3 projectileSpawnPosition = projectileSpawnPoint.position + transform.forward * 1.5f;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPosition, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = transform.forward * projectileSpeed;

        // Set the projectile's damage to the player's damage level
        MissileController missileController = projectile.GetComponent<MissileController>();
        if (missileController != null)
        {
            missileController.damage = this.damage;
            Debug.Log("Projectile damage: " + missileController.damage); // Print the damage of the missile
        }

    }

    // Call this method to upgrade the fire rate
    public void UpgradeFireRate()
    {
        // Increase the fire rate upgrade level
        fireRateUpgradeLevel++;

        // Decrease the fire rate (i.e., increase the speed of firing) based on the fire rate upgrade level
        fireRate = baseFireRate - fireRateUpgradeLevel * 0.05f;  // Adjust the multiplier as needed

        // Make sure fire rate is not less than some minimum value (for example, 0.1)
        fireRate = Mathf.Max(fireRate, 0.1f);
    }


    /*public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
   // public Animator animator; // Add this line to declare the Animator variable

    public float projectileSpeed = 10f;
    public float fireRate = 0.5f;

    private float nextFire = 0.0f;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            ShootProjectile();
            //animator.Play("Idle_gunMiddle_ar");
        }
    }

    private void ShootProjectile()
    {
        // Calculate the position of the projectile spawn point in front of the player
        Vector3 projectileSpawnPosition = projectileSpawnPoint.position + transform.forward * 1.5f;

        // Spawn the projectile at the calculated position
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPosition, Quaternion.identity);

        // Apply velocity to the projectile in the direction the player is facing
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = transform.forward * projectileSpeed;
        //animator.Play("Shoot_SingleShot_AR");
    }*/

}

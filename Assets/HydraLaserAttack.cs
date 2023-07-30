//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class HydraLaserAttack : MonoBehaviour
//{
//    public float laserSpeed = 5.0f;
//    public float zigzagSpeed = 2.0f;
//    public float zigzagAmplitude = 75.0f;

//    public GameObject laserBeamPrefab;
//    private Transform playerTransform;

//    public float laserAttackCooldown = 5.0f;
//    private float timeSinceLastLaserAttack = 0.0f;

//    void Start()
//    {
//        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
//    }

//    void Update()
//    {
//        // Update the timer
//        timeSinceLastLaserAttack += Time.deltaTime;

//        // Check if the cooldown has passed
//        if (timeSinceLastLaserAttack >= laserAttackCooldown)
//        {
//            // Call the FireLaser method
//            //FireLaser();

//            // Reset the timer
//            timeSinceLastLaserAttack = 0.0f;
//        }
//    }

//    public void FireLaser()
//    {
//        // Instantiate the laser beam prefab at the Hydra head's position
//        GameObject laserBeam = Instantiate(laserBeamPrefab, transform.position, Quaternion.identity);

//        // Set the laser beam's rotation to point towards the player
//        Vector3 direction = (playerTransform.position - transform.position).normalized;
//        Quaternion targetRotation = Quaternion.LookRotation(direction);
//        laserBeam.transform.rotation = targetRotation;

//        // Set the starting X rotation to 66
//        laserBeam.transform.eulerAngles = new Vector3(66, laserBeam.transform.eulerAngles.y, laserBeam.transform.eulerAngles.z);

//        // Attach the LaserBeamController component to the laser beam (if not already present)
//        LaserBeamController controller = laserBeam.GetComponent<LaserBeamController>();
//        if (controller == null)
//        {
//            controller = laserBeam.AddComponent<LaserBeamController>();
//        }

//        // Set the LaserBeamController parameters
//        controller.hydraHead = transform;
//        controller.laserSpeed = laserSpeed;
//        controller.zigzagSpeed = zigzagSpeed;
//        controller.zigzagAmplitude = zigzagAmplitude;
//    }
//}

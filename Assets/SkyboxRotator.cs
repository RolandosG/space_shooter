using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
     public Transform planet;  // Assign the planet transform in the inspector
    public float skyboxRotationSpeed = 1.2f;  // Set your desired skybox rotation speed here
    public float planetRotationSpeed = 0.8f;  // Set your desired planet rotation speed here
    public float rotationDuration = 2f;  // Set your desired rotation duration here
    public float minTimeBetweenRotations = 5f;  // Minimum time between rotations
    public float maxTimeBetweenRotations = 10f;  // Maximum time between rotations

    private float rotationEndTime;
    private float nextRotationTime;

    void Start()
    {
        ScheduleNextRotation();
    }

    void Update()
    {
        // If it's time for a rotation and the previous rotation has finished
        if (Time.time > nextRotationTime && Time.time > rotationEndTime)
        {
            StartCoroutine(RotateEnvironment());
            ScheduleNextRotation();
        }
    }

    private IEnumerator RotateEnvironment()
    {
        float rotationStart = Time.time;
        rotationEndTime = rotationStart + rotationDuration;
        float initialSkyboxRotation = RenderSettings.skybox.GetFloat("_Rotation");
        float initialPlanetRotation = planet.eulerAngles.y;
        float targetRotation = initialSkyboxRotation + Random.Range(-180f, 180f);  // Rotate between -180 and 180 degrees



        while (Time.time < rotationEndTime)
        {
            float t = (Time.time - rotationStart) / rotationDuration;  // Normalized time between 0 and 1
            float skyboxRotation = Mathf.Lerp(initialSkyboxRotation, targetRotation, t);
            float planetRotation = Mathf.Lerp(initialPlanetRotation, targetRotation, t * skyboxRotationSpeed / planetRotationSpeed);
            RenderSettings.skybox.SetFloat("_Rotation", skyboxRotation);
            planet.eulerAngles = new Vector3(planet.eulerAngles.x, planetRotation, planet.eulerAngles.z);
            yield return null;
        }
    }

    private void ScheduleNextRotation()
    {
        nextRotationTime = Time.time + Random.Range(minTimeBetweenRotations, maxTimeBetweenRotations);
    }
}

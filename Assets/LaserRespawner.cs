using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRespawner : MonoBehaviour
{
    public float respawnCooldown = 3.0f;

    private GameObject laserBeam;

    void Start()
    {
        laserBeam = transform.Find("HydraNeck/Head/LaserBeam").gameObject;
    }

    public void StartRespawn()
    {
        StartCoroutine(RespawnLaserAfterCooldown());
    }

    IEnumerator RespawnLaserAfterCooldown()
    {
        laserBeam.SetActive(false);
        yield return new WaitForSeconds(respawnCooldown);
        laserBeam.SetActive(true);
    }
}

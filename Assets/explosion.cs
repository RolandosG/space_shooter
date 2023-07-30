using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    private AudioSource explosionSound;

    void Start()
    {
        explosionSound = GetComponent<AudioSource>();

        // Destroy the game object after the sound has finished playing
        Destroy(gameObject, explosionSound.clip.length);
    }
}

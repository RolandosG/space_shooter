using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purchaseAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClickSound()
    {
        audioSource.Play();
    }

}

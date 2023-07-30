using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PersistAcrossScenes : MonoBehaviour
{
    public static PersistAcrossScenes Instance;
    private AudioSource audioSource;
    private const float defaultVolume = 0.14f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = defaultVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (this == Instance && audioSource != null)
        {
            audioSource.volume = defaultVolume;
        }
    }
}


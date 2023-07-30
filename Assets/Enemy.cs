using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDead = false;

    public AudioClip deathSound;
    private AudioSource audioSource;

    public delegate void EnemyDestroyedAction();
    public event EnemyDestroyedAction OnEnemyDestroyed;


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = deathSound;
        // Optional: configure your audioSource here (e.g., set volume, spatial blend, etc.)
    }

    private void OnDestroy()
    {
        if (OnEnemyDestroyed != null)
        {
            foreach (var d in OnEnemyDestroyed.GetInvocationList())
                OnEnemyDestroyed -= (EnemyDestroyedAction)d;
        }
    }

    // Call this when the enemy is defeated
    public virtual void Defeat()
    {
        
        Debug.Log("Defeat method called in Enemy class.");
        if (isDead) return;
        isDead = true;

        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        OnEnemyDestroyed?.Invoke();
        // Consider adding a delay before destroying the object if you want the sound to play fully.
        Destroy(gameObject, deathSound.length);  // This will destroy the enemy object after the sound duration
    }

}

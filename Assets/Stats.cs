using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    // keep track of entity health
    public float health = 20;
    public float speed;
    public float damage;
    public float score;
    public float currency;
    public float finalScore;
   
    private void Awake()
    {
        // If there's an existing instance of Stats and it's not this one
        if (instance != null && instance != this)
        {
            // Destroy the other instance so we only ever have one Stats instance
            Destroy(instance.gameObject);
        }

        // Set the instance to this object
        instance = this;

        // Don't destroy the Stats object when changing scenes
        DontDestroyOnLoad(gameObject);
    }
    public void UpdateHealth(float value)
    {
        health -= value;
    }
    public void IncrementScore(float value)
    {
        score += value;
    }
}

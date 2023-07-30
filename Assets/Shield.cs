using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip destroySound; // Sound when shield is destroyed
    private AudioSource audioSource;

    public int health;
    public Shielder parentShielder;

    private void Start()
    {
        parentShielder = GetComponentInParent<Shielder>();

        // Initialize the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;


        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Play the destroy sound
        if (destroySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destroySound);
        }

        if (parentShielder != null)
        {
            parentShielder.ShieldDestroyed(true);
        }
        Destroy(gameObject, destroySound.length); // This ensures the game object remains until sound has played
    }
}

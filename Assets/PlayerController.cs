using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;
    public static PlayerController instance;
    public PlayerShooting playerShooting;

    public HudManager hudManager;
    public Camera gameCamera;

    public Timer timer;

    public float speed;
    // DASH
    public float dashSpeed = 20f;
    public float dashDuration = 0.5f;

    private Rigidbody m_Rigidbody;
    private bool m_IsDashing = false;
    private float m_DashEndTime = 0f;
    private Vector3 m_DashDirection;

    public Transform leftWall;
    public Transform rightWall;

    private Stats m_Stats;
    private float m_RotationZ = 0.0f;
    private const float MAX_ROTATION_Z = 15.0f;
    private const float ROTATION_SPEED = 2.0f;

    public Transform cameraParent;

    public float missileSpeed = 20f;

    public GameObject explosionPrefab;
    public GameObject bulletPrefab;  // The bullet prefab

    private float maxHealth;  // The player's maximum health
    private float currentHealth;  // The player's current health

    public int baseDamage;
    private int damage = 10;

    public float baseFireRate;  // The base fire rate, set this to whatever value you want

    private float fireRate;  // The actual fire rate, this is the value that will be affected by the upgrade
    private float nextFireTime = 0;  // The time when the player can fire again

    private void Start()
    {
        playerData = SaveManager.Instance.Load();
        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.score = 0;
            playerData.currency = 0;
            playerShooting.damage = playerData.damage;
            playerData.hasDashAbility = false; // Initialize hasDashAbility to false

        }

        m_Stats.score = 0; // Reset the score for each game session
        UpdateMaxHealth();  // Initialize the player's health based on the saved data
        UpdateFireRate(); // Initialize the player's fire rate based on the saved data

        hudManager.UpdateHealthText(m_Stats.health);  // Update the HUD with the correct initial health value

    }

    private void Awake()
    {
        m_Stats = GetComponent<Stats>();
        hudManager.UpdateHealthText(m_Stats.health);
        hudManager.UpdateHealthText(m_Stats.score);
        cameraParent = transform.Find("CameraParent");
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a PlayerController.
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {   // DASH
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Calculate the movement direction
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
        // Apply the movement
        if (!m_IsDashing)
        {
            m_Rigidbody.velocity = movementDirection * speed;
        }
        // Check for dash input
        if (playerData.hasDashAbility && Input.GetKeyDown(KeyCode.Space) && !m_IsDashing) // Check if player has the dash ability
        {
            m_IsDashing = true;
            m_DashEndTime = Time.time + dashDuration;
            m_DashDirection = movementDirection;
            m_Rigidbody.velocity = m_DashDirection * dashSpeed;
        } 
        // End the dash if the duration is over
        if (m_IsDashing && Time.time >= m_DashEndTime)
        {
            m_IsDashing = false;
            m_Rigidbody.velocity = Vector3.zero;
        }
        if (m_Stats.health <= 0)
        {
            Die();
            FindObjectOfType<RestartGame>().GameOver();
        }

       // float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalPosition = transform.position.x + horizontalInput * speed * Time.deltaTime;
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;
        float playerSize = transform.localScale.x / 2;
        // WALLS
        if (leftWall != null && horizontalPosition - playerSize <= leftWall.position.x + leftWall.localScale.x / 2)
        {
            return;
        }
        if (rightWall != null && horizontalPosition + playerSize >= rightWall.position.x - rightWall.localScale.x / 2)
        {
            return;
        }

        // rotate player based on input
        if (horizontalInput > 0)
        {
            m_RotationZ = Mathf.Lerp(m_RotationZ, -MAX_ROTATION_Z, Time.deltaTime * ROTATION_SPEED);
        }
        else if (horizontalInput < 0)
        {
            m_RotationZ = Mathf.Lerp(m_RotationZ, MAX_ROTATION_Z, Time.deltaTime * ROTATION_SPEED);
        }
        else
        {
            m_RotationZ = Mathf.Lerp(m_RotationZ, 0.0f, Time.deltaTime * ROTATION_SPEED);
        }
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_RotationZ);
        // camera stability
        float rotation = horizontalInput * 0f; // Change 20f to control the rotation speed
        cameraParent.rotation = Quaternion.Euler(0f, 0f, -rotation);

        // move player based on input
        transform.position = new Vector3(horizontalPosition, 1, transform.position.z);


        

    }
    public void UpdateMaxHealth()
    {
        // Get the PlayerData from the SaveManager
        PlayerData playerData = SaveManager.Instance.Load();

        // Update the player's max health based on the health upgrade level
        maxHealth = 20 + playerData.healthUpgradeLevel * 2;

        // Update the player's actual health
        m_Stats.health = maxHealth;
    }

    public void UpgradeHealth()
    {
        // Get the PlayerData from the SaveManager
        PlayerData playerData = SaveManager.Instance.Load();

        // Increase the health upgrade level
        playerData.healthUpgradeLevel++;

        // Save the updated PlayerData
        SaveManager.Instance.Save(playerData);

        // Update the player's max health
        UpdateMaxHealth();

        // Update the player's health on the HUD
        hudManager.UpdateHealthText(m_Stats.health);
    }

    public void UpdateFireRate()
    {
        // Decrease the fire rate (i.e., increase the speed of firing) based on the fire rate upgrade level
        fireRate = baseFireRate - playerData.fireRateUpgradeLevel * 0.1f;  // Adjust the multiplier as needed

        // Make sure fire rate is not less than some minimum value (for example, 0.1)
        fireRate = Mathf.Max(fireRate, 0.1f);
    }
    public void UpgradeFireRate()
    {
        // Increase the fire rate upgrade level
        playerData.fireRateUpgradeLevel++;

        // Save the updated PlayerData
        SaveManager.Instance.Save(playerData);

        // Update the player's fire rate
        UpdateFireRate();
    }
    // RECIEVERS
    public void RecieveDamage()
    {
        m_Stats.UpdateHealth(20);
        hudManager.UpdateHealthText(m_Stats.health);
        Debug.Log("Ouch!");
        if (m_Stats.health <= 0)
        {
            timer.StopTimer();
            // Handle player death here (e.g., destroy the player, trigger game over, etc.)
            Die();
        }

    }
    public void TakeDamage(int damage)
    {
        m_Stats.UpdateHealth(damage);
            hudManager.UpdateHealthText(m_Stats.health);
        Debug.Log("Ouch!");
        if (m_Stats.health <= 0)
        {
            timer.StopTimer();
            Die();
        }

    }
    public void RecieveHealth()
    {
        m_Stats.UpdateHealth(10);
        hudManager.UpdateHealthText(m_Stats.health);
        Debug.Log("Health received!");
    }

    public void RecieveScore()
    {
        Debug.Log("m_Stats: " + m_Stats);
        Debug.Log("playerData: " + playerData);
        Debug.Log("scoreMultiplierLevel: " + playerData.scoreMultiplierLevel);
        Debug.Log("hudManager: " + hudManager);
        m_Stats.IncrementScore(10 * (playerData.scoreMultiplierLevel + 1));
        hudManager.UpdateScoreText(m_Stats.score);
        Debug.Log("Score!");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EvaderEnemy"))
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("EvaderEnemy"))
        {
            m_Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
        }
    }
    private void Die()
    {
        Debug.Log("Player died with score: " + m_Stats.score);

        PlayerData dataToSave = playerData;  // Use the existing PlayerData


        Debug.Log("Created PlayerData with score: " + dataToSave.score);

        // Add current game session's score to the cumulative score in PlayerData
        dataToSave.score += m_Stats.score;

        // Save the updated PlayerData
        SaveManager.Instance.Save(dataToSave);

        PlayerPrefs.SetFloat("FinalScore", m_Stats.score);
        PlayerPrefs.SetFloat("PlayerScore", m_Stats.score);
        Debug.Log("Die function called");

        if (SaveManager.Instance == null)
        {
            Debug.Log("SaveManager.Instance is null");
        }

        Debug.Log("Saved score: " + m_Stats.score);

        // Play death animation or spawn particle effect (if any)
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Camera.main.transform.parent = null;
        DisableAllEnemies();

        // Trigger game over logic
        FindObjectOfType<RestartGame>().GameOver();

        // Destroy the player object
        Destroy(gameObject);

        // Disable HUD
        hudManager.canvas.gameObject.SetActive(false);

       
    }

    public void ShootBullet()
    {
        // Check if the player can shoot
        if (Time.time >= nextFireTime)
        {
            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Get the BulletController
            PlayerShooting PlayerShooting = bullet.GetComponent<PlayerShooting>();

            // Set the bullet's damage
            PlayerShooting.damage = damage;

            // Set the time when the player can fire again
            nextFireTime = Time.time + fireRate;

        }
    }
    private void DisableAllEnemies()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.DisableScript();
        }
    }

    public PlayerData CreatePlayerData()
    {
        PlayerData data = new PlayerData();
        data.score = playerData.score; // This is the total score up until now
        data.currency = playerData.currency; // This should persist across sessions
        data.health = m_Stats.health;
        data.speed = speed;
        //data.dashEnabled = // get this information from wherever it is stored;
        return data;
    }

    public void LoadPlayerData(PlayerData data)
    {
        m_Stats.health = data.health;
        speed = data.speed;
        // Apply the dashEnabled value here
    }
}

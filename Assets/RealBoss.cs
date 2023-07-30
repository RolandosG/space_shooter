using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealBoss : MonoBehaviour
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float pulseSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackInterval;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject RingProjectilePrefab;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private Transform projectileSpawnPoint;

    private float currentScale;
    private bool scalingUp = true;
    private float currentRotation;
    private int attackCount = 0;
    private bool attacking = false;

    private int baseHealth; // this is your base health

    public delegate void BossDestroyed();
    public event BossDestroyed OnBossDestroyed;

    [SerializeField] private int health;
    private PlayerController m_Pc;

    private bool hasEntered = false;
    private bool readyToAttack = false;

    private bool enemiesDestroyed = false;

    private Enemy enemyInstance;
    private RealBoss realBossInstance;

    void Start()
    {

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }
        currentScale = minScale;
        currentRotation = transform.rotation.eulerAngles.z;
        StartCoroutine(MoveToYPosition(0, 25.0f));
        
        // initialize baseHealth
        baseHealth = health;
    }
    public void SetWaveNumber(int waveNumber)
    {
        // This will increase the health according to the wave number.
        // Modify this according to your needs.
        health = baseHealth * waveNumber;
    }
    void Update()
    {
        if (Mathf.Abs(transform.position.y - 0) <= 0.1f)
        {
            readyToAttack = true;
            if (!enemiesDestroyed)
            {
                FindObjectOfType<EnemyManager>().DestroyEnemies();
                enemiesDestroyed = true;
            }
        }

        if (!readyToAttack)
        {
            return;
        }
        if (!hasEntered)
        {
            StartCoroutine(MoveBossToStartPosition());
        }
        else
        {
            Pulse();
            MoveAndRotate();
            PerformAttacks();
        }
    }

    private IEnumerator MoveBossToStartPosition()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float entranceSpeed = 1.0f;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * entranceSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        transform.position = targetPosition;
        hasEntered = true;
    }
    private IEnumerator MoveToYPosition(float targetY, float moveSpeed)
    {
        while (Mathf.Abs(transform.position.y - targetY) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void Pulse()
    {
        if (scalingUp)
        {
            currentScale = Mathf.Lerp(currentScale, maxScale, pulseSpeed * Time.deltaTime);
            if (currentScale >= maxScale - 0.1f)
            {
                scalingUp = false;
            }
        }
        else
        {
            currentScale = Mathf.Lerp(currentScale, minScale, pulseSpeed * Time.deltaTime);
            if (currentScale <= minScale + 0.1f)
            {
                scalingUp = true;
            }
        }
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void MoveAndRotate()
    {
        if (!attacking)
        {
            float targetX = Mathf.PingPong(Time.time * moveSpeed, 14f) - 7f;
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

            if (attackCount % 2 == 0)
            {
                currentRotation += 30f;
                if (currentRotation >= 360f)
                {
                    currentRotation -= 360f;
                }
            }
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
        }
    }

    private void PerformAttacks()
    {
        if (!attacking && (currentRotation % 120f == 0f) && currentRotation != 0f)
        {
            attacking = true;
            StartCoroutine(Attack());
            RingProjectilePrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private IEnumerator Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = -projectileSpawnPoint.forward * 10f;
        yield return new WaitForSeconds(1f);

        attacking = false;
        attackCount++;
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
        m_Pc?.RecieveScore();
        OnBossDestroyed?.Invoke();
        Destroy(gameObject);
    }
    
}

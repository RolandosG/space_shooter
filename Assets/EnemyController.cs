using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Catcher,
    Evader,
    RedEvader,
    MiniBoss,
    MainBoss
}

public class EnemyController : MonoBehaviour
{
    //Death Animation
    public GameObject particlePrefab;
    //private bool isDestroyed = false;
    //Stats script
    private Stats m_Stats;
    //Movement
    public float speed;
    //Enemy
    public EnemyType enemyType;

    private PlayerController m_Pc;
    private float m_ThreshHoldPositionZ = -78.0f;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            m_Pc = playerObject.GetComponent<PlayerController>();
        }
    }
    private void Update()
    {   

        // enemy speed
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);

        // check distance between player and enemy transform
        if (m_Pc != null)
        {
            if (m_Pc != null && Vector3.Distance(m_Pc.transform.position, transform.position) < 1.0f)
            {
                if (enemyType == EnemyType.Evader || enemyType == EnemyType.RedEvader)
                {
                    m_Pc.RecieveDamage();
                }
                else if (enemyType == EnemyType.Catcher)
                {
                    m_Pc.RecieveHealth();
                }
                Destroy(gameObject);

            }
            else if (m_Pc.transform.position.z - transform.position.z > 5.0f && enemyType == EnemyType.Catcher)
            {
                m_Pc.RecieveDamage();
                Destroy(gameObject);
            }
            else if (transform.position.z <= m_ThreshHoldPositionZ) // if threshold of player is passed enemy gets destroyed
            {
                Destroy(gameObject);
            }
        }
        


    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Projectile"))
        {

            if (collision.gameObject != null && collision.gameObject.CompareTag("Projectile"))
            {
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
                //isDestroyed = true;
                if (m_Pc != null) // add null check here
                {
                    m_Pc.RecieveScore();
                }
                Destroy(gameObject);
            }
        }
        
    }
    public void DisableScript()
    {
        enabled = false;
    }
}
    

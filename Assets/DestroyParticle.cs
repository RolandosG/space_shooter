using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public GameObject particlePrefab;
    public float destroyDelay = .0f;

    private void OnDisable()
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

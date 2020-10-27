using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEnemySpawn : MonoBehaviour
{

    public GameObject enemy;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
    }

    public void SpawnEnemy()
    {
        Instantiate(enemy, transform.position + transform.TransformVector(UnityEngine.Random.Range(-4, 4), 0, UnityEngine.Random.Range(-4, 4)), transform.rotation);
        if(stopSpawning)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
}

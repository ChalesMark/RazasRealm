using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject initialSpawnGroup;
    public int baseEnemyCount = 0;
    public float timeBetweenWaves = 3.0f;
    public float timeBetweenSpawns = 2.0f;


    private List<Transform> spawnPoints;
    private List<GameObject> enemies;
    private Text waveText; 
    private Text enemiesRemainingText;
    private int currentWave;
    private int enemiesSpawnedThisWave;
    private int totalEnemiesThisWave;
    private int enemiesRemaining;
    private bool finishedSpawning = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetSpawnPoints();
        enemies = GetEnemiesToSpawn();
        currentWave = 0;
        enemiesSpawnedThisWave = 0;
        totalEnemiesThisWave = baseEnemyCount;
        waveText = Camera.main.transform.Find("Canvas").Find("WaveText").GetComponent<Text>();
        waveText.enabled = false;
        enemiesRemainingText = Camera.main.transform.Find("Canvas").Find("EnemyCount").GetComponent<Text>();
        enemiesRemainingText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining + " / " + enemiesSpawnedThisWave;

        if (!finishedSpawning && enemiesSpawnedThisWave == totalEnemiesThisWave && totalEnemiesThisWave != 0)
            StopSpawning();
        else if (finishedSpawning && enemiesRemaining == 0)
            StartNextWave();
    }

    List<Transform> GetSpawnPoints()
    {
        List<Transform> spawns = new List<Transform>();
        if (initialSpawnGroup != null)
        {
            foreach (Transform spawn in initialSpawnGroup.transform)
                spawns.Add(spawn);
        }

        return spawns;
    }

    List<GameObject> GetEnemiesToSpawn()
    {
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        if (Enemy1 != null)
            enemiesToSpawn.Add(Enemy1);
        if (Enemy2 != null)
            enemiesToSpawn.Add(Enemy2);
        if (Enemy3 != null)
            enemiesToSpawn.Add(Enemy3);
        return enemiesToSpawn;

    }

    public void StartFirstWave()
    {
        waveText.enabled = true;
        enemiesRemainingText.enabled = true;
        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;
        if (currentWave > PlayerPrefs.GetInt("Highscore"))
            PlayerPrefs.SetInt("Highscore", currentWave);
        MoveWaveCounterToCenter();
        Invoke("MoveWaveCounterToBottomRight", 3);
        waveText.text = "Wave " + currentWave;
        enemiesSpawnedThisWave = 0;
        if (currentWave != 1)
            totalEnemiesThisWave += 5;
        if (timeBetweenSpawns >= 1.0f)
            timeBetweenSpawns -= 0.1f;
        StartSpawning();
    }

    public void StartSpawning()
    {
        finishedSpawning = false;
        if(currentWave == 1)
            InvokeRepeating("SpawnEnemy", 0, timeBetweenSpawns);
        else
            InvokeRepeating("SpawnEnemy", timeBetweenWaves, timeBetweenSpawns);
    }

    public void StopSpawning()
    {
        finishedSpawning = true;
        CancelInvoke("SpawnEnemy");
    }

    public void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemy = enemies[Random.Range(0, enemies.Count)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemiesSpawnedThisWave++;
    }

    public void Restart()
    {
        StopSpawning();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
        currentWave = 0;
        totalEnemiesThisWave = baseEnemyCount;
        finishedSpawning = false;
        waveText.enabled = false;
        enemiesRemainingText.enabled = false;
        timeBetweenSpawns = 3.0f;
    }

    public void MoveWaveCounterToCenter()
    {
        waveText.fontSize = 70;
        RectTransform position = Camera.main.transform.Find("Canvas").Find("WaveText").GetComponent<RectTransform>();
        position.anchorMin = new Vector2(0.5f, 0.5f);
        position.anchorMax = new Vector2(0.5f, 0.5f);
        position.anchoredPosition = new Vector3(60.0f, 170.0f, 0.0f);
    }

    public void MoveWaveCounterToBottomRight()
    {
        waveText.fontSize = 50;
        RectTransform position = Camera.main.transform.Find("Canvas").Find("WaveText").GetComponent<RectTransform>();
        position.anchorMin = new Vector2(1.0f, 0.0f);
        position.anchorMax = new Vector2(1.0f, 0.0f);
        position.anchoredPosition = new Vector3(-50.0f, 20.0f, 0.0f);
    }

    public void AddSpawns(GameObject spawnGroup)
    {
        foreach (Transform spawn in spawnGroup.transform)
            spawnPoints.Add(spawn);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    private void OnDestroy()
    {
        Camera.main.transform.Find("Canvas").Find("WaveText").GetComponent<Text>().enabled = false;
        Camera.main.transform.Find("Canvas").Find("EnemyCount").GetComponent<Text>().enabled = false;
    }

}

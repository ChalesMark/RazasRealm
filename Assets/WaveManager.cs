using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject baseEnemy;

    public int baseEnemyCount;
    private List<Transform> spawnPoints;
    private Text waveText;

    private int currentWave;
    private int enemiesSpawnedThisWave;
    private int totalEnemiesThisWave;
    private bool finishedSpawning = false;
    private int enemiesRemaining;
    private float timeBetweenSpawns = 3.0f;

    private Text enemiesRemainingText;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetSpawnPoints();
        currentWave = 0;
        enemiesSpawnedThisWave = 0;
        totalEnemiesThisWave = baseEnemyCount;
        waveText = Camera.main.transform.Find("Canvas").Find("WaveText").GetComponent<Text>();
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
        GameObject spawnGroup = GameObject.Find("EnemySpawns");
        foreach (Transform spawn in spawnGroup.transform)
            spawns.Add(spawn);

        return spawns;
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
        MoveWaveCounterToCenter();
        Invoke("MoveWaveCounterToBottomRight", 3);
        waveText.text = "Wave " + currentWave;
        totalEnemiesThisWave += 5;
        enemiesSpawnedThisWave = 0;
        if (timeBetweenSpawns >= 1.0f)
            timeBetweenSpawns -= 0.1f;
        StartSpawning();
    }

    public void StartSpawning()
    {
        finishedSpawning = false;
        if(currentWave == 1)
            InvokeRepeating("SpawnEnemy", 1, timeBetweenSpawns);
        else
            InvokeRepeating("SpawnEnemy", 5, timeBetweenSpawns);
    }

    public void StopSpawning()
    {
        finishedSpawning = true;
        CancelInvoke("SpawnEnemy");
    }

    public void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(baseEnemy, spawnPoint.position, spawnPoint.rotation);
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

    public int GetCurrentWave()
    {
        return currentWave;
    }

}

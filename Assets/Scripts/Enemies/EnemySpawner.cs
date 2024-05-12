using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // A list of groups of enemies to spawn in this wave
        public int waveQouta;   // The total number of enemies to sapwn in this wave
        public float spawnInterval; // The interval at which to spawn enemies
        public int spawnCount;  // The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;  // The number of enemies to spawn in this wave
        public int spawnCount;  // The number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // A list of all the waves in the game
    public int currentWaveCount; // The index of the current wave (Starts from 0)

    [Header("Spawner Attributes")]
    float spawnTimer = 0; // Timer used to determine when to sapwn the next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed; // The maximum number of enemies allowed on the map at once
    public bool maxEnemiesReached = false; // A flag indicating if the maximum number of enemies has been reached
    public float waveInterval; // The interval between each wave
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; // A list to store al the relative spawn points of enemies

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQouta();
    }

    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive) // Check if the wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        // check if its time to spawne the next enemy
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            isWaveActive = false;
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;

        // Wait for 'waveInterval' seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        // IOf there are more waves to start after the current wave, move on to the next wave
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQouta();
        }
    }

    void CalculateWaveQouta()
    {
        int currentWaveQouta = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQouta += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQouta = currentWaveQouta;
        Debug.LogWarning(currentWaveQouta);
    }

    /// <summary>
    /// This method will stop spawning enemies if the amount of enemies on the map is maximum
    /// This method will only spawn enemies in a particular wave until it is time fo the next waves enemies to be spawned
    /// </summary>

    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQouta && !maxEnemiesReached)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    // Call this function when an enemy is killed
    public void OnEnemyKill()
    {
        // Decrement the number of enemies alive
        enemiesAlive--;

        // Reset the maxEnemiesReached flag if the number of enemies alive has deopped below the maximum amount
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}

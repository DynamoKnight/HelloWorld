using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    private GameObject gm;
    private MultiplayerManager mm;

    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float spawnRate = 3;
    [SerializeField] private float waveRate = 10;

    // 2 possible players
    private GameObject player1;
    private GameObject player2; 

    // List of all enemies
    [SerializeField] private List<GameObject> enemyPrefabs;
    private List<GameObject> allEnemies;
    // The ones considered to spawn
    private List<GameObject> currentEnemies = new();
    // Number of enemies currently active
    private int enemyCount;
    // Enemies left to spawn
    private int enemiesToSpawn = 0;

    private int waveNumber = 0;
    // Indicates that a wave is in progress
    private bool startingWave;
    // Indicates wheter an enemy is done spawning
    protected bool enemySpawning = false;
    
    void Start(){
        gm = GameObject.Find("GameManager");
        mm = gm.GetComponent<MultiplayerManager>();

        // Duplicate to prevent error
        allEnemies = enemyPrefabs;
        // Only 1 enemy at start
        currentEnemies.Add(allEnemies[0]);
        allEnemies.RemoveAt(0);
    }

    // Update is called once per frame
    void FixedUpdate(){
        // If the players exists, spawn enemies
        player1 = GameObject.FindGameObjectWithTag("Player");
        //player2 = GameObject.FindGameObjectWithTag("Player2");
        // Will only spawn if game is unpaused, functional, and an enemy is not already being spawned
        if ((player1 || player2) && !enemySpawning && LevelManager.instance.spawnEnemies && !LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Gets all currently alive enemies
            enemyCount = FindObjectsOfType<Enemy>().Length;
            // If the wave ended with all enemies are dead and no longer spawning
            if (enemyCount == 0 && enemiesToSpawn == 0 && !startingWave){
                // Proceeds to next wave after some time
                mm.CompleteWave(waveNumber);
                // Gives hearts to player
                Player p1 = player1.GetComponent<Player>();
                if (p1.GetHearts() < p1.GetMaxHealth()){
                    p1.AddHeart();
                }
                waveNumber++;
                StartCoroutine(StartWave(waveNumber, waveRate));
            }
            // Starts spawning enemies one at a time, has to wait for other enemy to finish spawning
            if (enemiesToSpawn > 0 && !enemySpawning){
                StartCoroutine(SpawnEnemy());
                enemiesToSpawn -= 1;
            }
        } 
    }

    // Starts the spawning process
    private IEnumerator StartWave(int waveNumber, float duration){
        startingWave = true;
        // Waits before starting next wave
        yield return new WaitForSeconds(duration);
        mm.StartWave(waveNumber);
        // Takes a prefab from all the enmies and adds it to the spawn list
        // Every 3 waves, a new enemy is introduced
        if (waveNumber % 3 == 0 && allEnemies.Count > 0){
            currentEnemies.Add(allEnemies[0]);
            allEnemies.RemoveAt(0);
        }
        enemiesToSpawn = waveNumber;
        startingWave = false;
    }

    // Spawns an enemy
    IEnumerator SpawnEnemy(){
        enemySpawning = true;

        // Spawns a random enemy at a random spawn point
        GameObject alien = currentEnemies[Random.Range(0, currentEnemies.Count)];
        Instantiate(alien, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, alien.transform.rotation);
        yield return new WaitForSeconds(spawnRate);

        enemySpawning = false;
    }
    
}

    
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 40;
    private float spawnRate;
    [SerializeField] private int stage = 1;
    private float timer = 0f;
    private bool enemySpawned = false;

    private GameObject player;

    public GameObject[] alienPrefabs;

    void FixedUpdate(){
        // Only collects time if unpaused
        if(!LevelManager.instance.isPaused){
            timer += Time.deltaTime;
            spawnRate = CalculateSpawnRate(stage, timer);
        }
        // If the player exists, spawn enemies
        player =  GameObject.Find("Player");
        // Will only spawn if game is unpaused, functional, and an enemy is not already being spawned
        if (player && LevelManager.instance.spawnEnemies && !enemySpawned && !LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            StartCoroutine(Spawn());
        }
    }

    // Spawns aliens
    IEnumerator Spawn(){
        enemySpawned = true;
        // Gets players position
        Vector2 spawnPos = player.transform.position;
        // The position will be outside the radius of the player
        spawnPos += Random.insideUnitCircle.normalized * spawnRadius;
        // Spawns a random object
        var alien = alienPrefabs[Random.Range(0, alienPrefabs.Length)];
        Instantiate(alien, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(spawnRate);
        // Allows to spawn again
        enemySpawned = false;
    }

    // Difficulty curves function by Lukas
    // stage increases frequency and time increases amplitude
    float CalculateSpawnRate(int stage, float time){
        return (float)(Math.Sin(Math.Log(stage + Math.E) * time) * Math.Log(stage * time) + Math.Log(stage * time + Math.E));
    }
}

    
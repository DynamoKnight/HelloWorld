using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float spawnRadius = 40;
    private float spawnRate;
    [SerializeField]
    private int stage = 1;
    private float timer = 0f;

    public GameObject[] alienPrefabs;

    // Start is called before the first frame update
    void Start(){
        StartCoroutine(Spawn());
    }

    void Update(){
        // Only collects time if unpaused
        if(!LevelManager.instance.isPaused){
            timer += Time.deltaTime;
            spawnRate = CalculateSpawnRate(stage, timer);
        }
    }

   IEnumerator Spawn(){
        // Will only spawn if game is functional
        if (LevelManager.instance.isFunctional){
            // Gets players position
            GameObject player =  GameObject.Find("Player");
            if (player){
                Vector2 spawnPos = player.transform.position;
                // The position will be outside the radius of the player
                spawnPos += Random.insideUnitCircle.normalized * spawnRadius;
                // Spawns a random object
                var alien = alienPrefabs[Random.Range(0, alienPrefabs.Length)];
                Instantiate(alien, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(spawnRate);
                // Spawns again
                StartCoroutine(Spawn());
            }
        }
   }

    // Difficulty curves function by Lukas
    // stage increases frequency and time increases amplitude
   float CalculateSpawnRate(int stage, float time){
        return (float)(Math.Sin(Math.Log(stage + Math.E) * time) * Math.Log(stage * time) + Math.Log(stage * time + Math.E));
    }
}

    
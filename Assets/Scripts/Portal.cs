using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Portal : Collidable
{
    // Array of Scenes
    public string[] sceneNames;
    public GameObject[] alienPrefabs;
    public float spawnRate = 5f;
    public float spawnTimer;

    protected override void Update(){
        base.Update();
        if(spawnTimer >= spawnRate){
            spawnAlien();
            spawnTimer = 0f;
        }
        else{
            spawnTimer += Time.deltaTime;
        }
    }
    protected override void OnCollide(Collider2D coll){
        if (coll.name == "Player"){
            // Teleports the player to a random scene
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        }
    }

    private void spawnAlien(){
        var alien = alienPrefabs[Random.Range(0, alienPrefabs.Length)];
        // Creates an alien object
        Instantiate(alien, transform.position, transform.rotation);
    }
}

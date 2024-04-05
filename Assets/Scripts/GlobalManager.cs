using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    // Global
    public static GlobalManager instance;

    public float timePlayed;
    public int enemiesDefeated;
    public int planetsDiscovered;
    public int itemsCollected;

    public float lastDeath;

    void Awake(){
        // Makes sure there is 1 instance of this
        if (GlobalManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }

        timePlayed = 0;
        enemiesDefeated = 0;
        planetsDiscovered = 0;
        itemsCollected = 0;

        lastDeath = 0;

    }

    void Update(){
        timePlayed = Time.time - lastDeath;
    }
}

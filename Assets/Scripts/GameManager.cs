using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timePlayed;
    public int enemiesDefeated;
    public int planetsDiscovered;
    public int itemsCollected;

    public float lastDeath;

    void Awake(){
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

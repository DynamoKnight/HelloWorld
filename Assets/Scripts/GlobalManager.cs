using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public float gameStart;

    // Images of each planet
    [SerializeField] private Sprite[] images;
    
    void Awake(){
        // Makes sure there is 1 instance of this
        if (GlobalManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        // Statistics
        timePlayed = 0;
        enemiesDefeated = 0;
        planetsDiscovered = PlayerStats.PlanetsDiscovered;
        itemsCollected = 0;

        lastDeath = 0;
    }

    void Update(){
        // Updates time when game is active
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // The time taken for the level
            timePlayed = Time.time - gameStart;
            // The total time taken for the entire game
            PlayerStats.TimePlayed += Time.deltaTime;
        }
        
    }
    
    // Returns the dictionary of planet information
    public Dictionary<string, string[]> GetPlanets(){
        return PlayerStats.PlanetInfos;
    }
    // Returns the Planet Images
    public Sprite[] GetImages(){
        return images;
    }

    // Writes the stats into these game objects
    public void WriteStats(GameObject statsPanel){
        // Gets the text objects (inside the label object)
        GameObject timePlayedText = statsPanel.transform.GetChild(0).GetChild(0).gameObject;
        GameObject enemiesDefeatedText = statsPanel.transform.GetChild(1).GetChild(0).gameObject;
        GameObject planetsDiscoveredText = statsPanel.transform.GetChild(2).GetChild(0).gameObject;
        GameObject itemsCollectedText = statsPanel.transform.GetChild(3).GetChild(0).gameObject;

        timePlayedText.GetComponent<TMP_Text>().text = FormatTime(timePlayed);
        enemiesDefeatedText.GetComponent<TMP_Text>().text = enemiesDefeated.ToString();
        planetsDiscoveredText.GetComponent<TMP_Text>().text = planetsDiscovered.ToString();
        itemsCollectedText.GetComponent<TMP_Text>().text = itemsCollected.ToString();
    }

    // Rewrites for better format
    // time is in seconds
    public string FormatTime(float time){
        int minute = Convert.ToInt32(Math.Floor(time/60f));
        int seconds = Convert.ToInt32(time - minute*60);
        // Adds 0 if needed
        if (seconds < 10){
            return minute + ":0" + seconds;
        }
        else{
            return minute + ":" + seconds;
        }
    }

    // Resets current level data
    public void ResetStats(){
        timePlayed = 0;
        enemiesDefeated = 0;
        itemsCollected = 0;
        lastDeath = 0;
    }
}

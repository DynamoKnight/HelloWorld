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
    // Every planet with information about it
    // Array with key-value pairs
    // Planet, [Difficulty, Nickname, Info, Enemies, Temperature]
    public Dictionary<string, string[]> infos = new Dictionary<string, string[]>
        {
            { "Pluto", new string[] {
                "Easy",
                "The Dwarf Planet",
                "The icy terrain and rocky landscape make it hard to navigate. While it's cold environment provides a challenge, the aliens settlers have made the planet a bit more bearable... ",
                "They have adapted to the cold climate and are not kind to intruders...",
                "-375"
                } },
            { "Neptune", new string[] {
                "Easy",
                "The Blue Giant",
                "Neptune has 14 known moons, the largest of which is Triton, which is believed to be a captured Kuiper Belt object.",
                "Frost bites ",
                "-330"
                } },
            { "Uranus", new string[] {
                "Medium",
                "The Bull's Eye Planet",
                "Uranus is the only planet in the solar system that rotates on its side",
                "Shoots balls",
                "-320"
                } },
            { "Saturn", new string[] {
                "Medium",
                "The Ringed Planet",
                "Saturn's density is so low that if there were a large enough body of water, it would float in it",
                "Magnetic fields",
                "-220"
                } },
            { "Jupiter", new string[] {
                "Hard",
                "The Gas Giant",
                "Jupiter's Great Red Spot is a massive storm that has been raging for at least 400 years and is large enough to engulf Earth two or three times over.",
                "Big boys",
                "-166"
                } },
            { "Mars", new string[] {
                "Hard",
                "The Red Planet",
                "Mars has the largest volcano and canyon in the solar system, Olympus Mons and Valles Marineris respectively",
                "Sand attacks",
                "-85"
                } },
            { "The Moon", new string[] {
                "Extreme",
                "Luna Selene",
                "One small step for man. One giant leap for mankind.",
                "Everybody",
                "0"
                } },
            { "Earth", new string[] {
                "Boss",
                "The Blue Planet",
                "The home of the advanced species classified as Humans.",
                "Boss is here",
                "59"
                } },
            { "Venus", new string[] {
                "Extreme",
                "The Morning Star",
                "Hot.",
                "Robots",
                "867"
                } },
            { "Mercury", new string[] {
                "Extreme",
                "The Swift Planet",
                "Close to Sun.",
                "Firey",
                "333"
                } }

            
        };
    
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
        planetsDiscovered = 0;
        itemsCollected = 0;

        lastDeath = 0;
    }

    void Update(){
        // Updates time when game is active
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            timePlayed = Time.time - gameStart;
        }
        
    }
    
    // Returns the dictionary of planet information
    public Dictionary<string, string[]> GetPlanets(){
        return infos;
    }
    // Returns the Planet Images
    public Sprite[] GetImages(){
        return images;
    }
    // Returns the Index of the Planet
    public int GetIdxOfPlanet(string planet){
        return infos.Keys.ToList().IndexOf(planet);
    }
    // Returns the Index of the Current Planet
    public int GetIdxOfCurrentPlanet(){
        return GetIdxOfPlanet(LevelManager.instance.currentPlanet);
    }

    // Writes the stats into these game objects and saves them
    public void WriteStats(GameObject statsPanel){
        // Gets the text objects (inside the label object)
        GameObject timePlayedText = statsPanel.transform.GetChild(0).GetChild(0).gameObject;
        GameObject enemiesDefeatedText = statsPanel.transform.GetChild(1).GetChild(0).gameObject;
        GameObject planetsDiscoveredText = statsPanel.transform.GetChild(2).GetChild(0).gameObject;
        GameObject itemsCollectedText = statsPanel.transform.GetChild(3).GetChild(0).gameObject;

        // Rewrites for better format
        float i = timePlayed;
        int minute = Convert.ToInt32(Math.Floor(i/60f));
        int seconds = Convert.ToInt32(i - minute*60);
        // Adds 0 if needed
        if (seconds < 10){
            timePlayedText.GetComponent<TMP_Text>().text = minute + ":0" + seconds;
        }
        else{
            timePlayedText.GetComponent<TMP_Text>().text = minute + ":" + seconds;
        }
        
        enemiesDefeatedText.GetComponent<TMP_Text>().text = enemiesDefeated.ToString();
        planetsDiscoveredText.GetComponent<TMP_Text>().text = planetsDiscovered.ToString();
        itemsCollectedText.GetComponent<TMP_Text>().text = itemsCollected.ToString();

        // Saves the data to the data of the current planet
        //Find best time played for the specific planet
        //Add enemies defeated to the total of all planets
        //Don't store planets discovered
        //Don't store items collected
    }
}

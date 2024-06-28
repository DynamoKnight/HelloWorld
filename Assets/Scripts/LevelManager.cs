using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the current level
public class LevelManager : MonoBehaviour
{
    // This is a public instance of LevelManager 
    // so that it can be used anytime
    public static LevelManager instance;

    // Indicates whether the game is paused
    public bool isPaused;
    // Indicates whether the User can interact with the player
    public bool isFunctional;
    // Indicates whether the current level is completed
    public bool levelComplete = false;
    // Indicates if enemies can spawn
    public bool spawnEnemies;

    public List<string> levels;
    // Represents the furthest planet that has been reached
    public string bestPlanet;
    // Represents the current planet it is on
    public string currentPlanet;

    private GameObject gm;
    private InventoryManager inventoryManager;
    private UIManager uiManager;
    public DeathPanel deathPanel;

    // Gets called when loading 
    private void Awake(){
        // Makes sure there is 1 instance of this (in a scene)
        if (LevelManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        levels = PlayerStats.PlanetInfos.Keys.ToList();
        bestPlanet = PlayerStats.BestPlanet;
        currentPlanet = levels[0];
        // Makes sure correct planet
        var currentScene = SceneManager.GetActiveScene().name;
        // Makes sure the scene is a planet
        if (levels.Contains(currentScene)){
            currentPlanet = currentScene;
            PlayerStats.CurrentPlanet = currentPlanet;
            // Sets the new bestPlanet if its higher index 
            if (GetIdxOfPlanet(currentScene) > GetIdxOfBestPlanet()){
                bestPlanet = currentScene;
                PlayerStats.BestPlanet = bestPlanet;
            }
        }
        Debug.Log("Current Scene: " + currentScene + "\nCurrent Planet: " + currentPlanet);
        
        // Initial conditions
        isPaused = false;
        isFunctional = false;
        spawnEnemies = true;
        
        gm = gameObject;
        inventoryManager = gm.GetComponent<InventoryManager>();
        uiManager = gm.GetComponent<UIManager>();
    }


    // Stops game and records data
    public void GameOver(){
        isFunctional = false;
        PlayerStats.TotalViolations += 1;
        PlayerStats.EnemiesDefeated += GlobalManager.instance.enemiesDefeated;
        // Everything is lost, except credits
        Inventory.ClearResources();
        // Shows death panel
        uiManager.ToggleDeathPanel();
        // Writes data onto the stats panel
        deathPanel.Die();
    }

    // Pauses and Unpauses the game
    public void TogglePause(){
        // Unpause
        if (Time.timeScale == 0){
            isPaused = false;
            Time.timeScale = 1;
        }
        // Pause
        else {
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    // Closes the game
    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }

    // Returns the next planet in order
    public string NextPlanet(){
        int idx = levels.IndexOf(bestPlanet); 
        string nextPlanet = levels[idx + 1];
        return nextPlanet;
    }

    // Saves data to know before changing scenes
    public void UpdateStats(){
        // Next Planet is unlocked
        PlayerStats.BestPlanet = NextPlanet();
        // Stores data
        PlayerStats.PlanetsDiscovered += 1;
        PlayerStats.EnemiesDefeated += GlobalManager.instance.enemiesDefeated;
    }

    // Returns the Index of the Planet
    public int GetIdxOfPlanet(string planet){
        return PlayerStats.PlanetInfos.Keys.ToList().IndexOf(planet);
    }
    // Returns the Index of the Best Planet
    public int GetIdxOfBestPlanet(){
        return GetIdxOfPlanet(bestPlanet);
    }
    // Returns the Index of the Current Planet
    public int GetIdxOfCurrentPlanet(){
        return GetIdxOfPlanet(currentPlanet);
    }

}

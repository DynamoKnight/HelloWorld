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
    // Planet level names
    public List<string> levels;
    public string currentPlanet;

    public Inventory inventory;
    public DeathPanel deathManager;

    // Gets called when loading 
    private void Awake(){
        // Makes sure there is 1 instance of this (in a scene)
        if (LevelManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        levels = new List<string> {"Pluto","Neptune","Uranus","Saturn","Jupiter","Mars","The Moon","Earth","Venus","Mercury","Multiplayer"};
        // if current scene is pluto
        currentPlanet = PlayerStats.CurrentPlanet;
        // Makes sure correct planet
        string level = SceneManager.GetActiveScene().name;
        Debug.Log(level);
        if (level != currentPlanet && levels.Contains(level)){
            currentPlanet = level;
        }

        // Initial conditions
        isPaused = false;
        isFunctional = false;
        spawnEnemies = true;
        
    }


    // Hides the UI of the deathpanel
    public void GameOver(){
        isFunctional = false;
        PlayerStats.TotalViolations += 1;
        PlayerStats.EnemiesDefeated += GlobalManager.instance.enemiesDefeated;
        UIManager _ui = GetComponent<UIManager>();
        if (_ui){
            _ui.ToggleDeathPanel();
            deathManager.Die();
        }
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
        int idx = levels.IndexOf(currentPlanet); 
        string nextPlanet = levels[idx + 1];
        return nextPlanet;
    }

    // Saves data to know before changing scenes
    public void UpdateStats(){
        PlayerStats.CurrentPlanet = NextPlanet();
        PlayerStats.PlanetsDiscovered += 1;
        PlayerStats.EnemiesDefeated += GlobalManager.instance.enemiesDefeated;

        // Gets the best time
        float bestTime = PlayerStats.BestTimes[GlobalManager.instance.GetIdxOfCurrentPlanet()];
        float currentTime = GlobalManager.instance.timePlayed;
        // Changes best time to whichever was fastest
        if (currentTime < bestTime || bestTime == 0){
            bestTime = currentTime;
        }
        // Sets new best time
        PlayerStats.BestTimes[GlobalManager.instance.GetIdxOfCurrentPlanet()] = bestTime;
        Debug.Log(PlayerStats.BestTimes[GlobalManager.instance.GetIdxOfCurrentPlanet()]);
    }

}

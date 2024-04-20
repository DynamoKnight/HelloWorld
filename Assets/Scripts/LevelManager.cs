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
        // Makes sure there is 1 instance of this
        if (LevelManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        levels = new List<string> {"Pluto","Neptune","Uranus","Saturn","Jupiter","Mars","The Moon","Earth"};
        // if current scene is pluto
        currentPlanet = levels[0];

        // Initial conditions
        isPaused = false;
        isFunctional = false;
        spawnEnemies = true;
        
    }

    void Update(){
        // The currentPlanet updates only when the scene is a planet
        string currentScene = SceneManager.GetActiveScene().name;
        if (levels.Contains(currentScene)){
            currentPlanet = currentScene;
        }

        if (currentPlanet == "Pluto"){
            GlobalManager.instance.planetsDiscovered = 1;
        }
        if (currentPlanet == "Neptune"){
            GlobalManager.instance.planetsDiscovered = 2;
        }
        if (currentPlanet == "Uranus"){
            GlobalManager.instance.planetsDiscovered = 3;
        }
    }

    // Hides the UI of the deathpanel
    public void GameOver(){
        isFunctional = false;
        UIManager _ui = GetComponent<UIManager>();
        if (_ui){
            deathManager.Die();
            _ui.ToggleDeathPanel();
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

}

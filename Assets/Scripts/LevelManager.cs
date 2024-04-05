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
    // Planet level names
    public List<string> levels;
    public string currentPlanet;

    /*public List<MissionItemList> MissionItemsByPlanet;
    public List<int> amountofEachItem;
    public List<MissionItemImageList> MissionItemImagesByPlanet;
    public List<MissionItemDropList> MissionItemDropsByPlanet;

    // Allows inspector input to be a list of lists
    [System.Serializable]
    public class MissionItemList{
        public List<string> MissionItems;
    }

    [System.Serializable]
    public class MissionItemImageList{
        public List<Sprite> MissionItemImages;
    }

    [System.Serializable]
    public class MissionItemDropList{
        public List<GameObject> MissionItemDrops;
    }*/

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
        if (SceneManager.GetActiveScene().name == "Pluto") {
            ToPlanet(0);
        }
        
    }

    void Start(){
        isPaused = false;
        isFunctional = true;
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

    // Reconfigures the inventory for the next level
    public void ToPlanet(int index){
        /*
        ClearAmount();
        inventory.missionItemsCollected = false;
        gameManager.planetsDiscovered += 1;
        for(int i = 0; i < MissionItemsByPlanet[index].MissionItems.Count; i++){
            inventory.missionItems = MissionItemsByPlanet[index].MissionItems;
            inventory.missionImages = MissionItemImagesByPlanet[index].MissionItemImages;
            inventory.missionItemDrops = MissionItemDropsByPlanet[index].MissionItemDrops;
            inventory.numberOfEachMissionItem[i] = amountofEachItem[i];
        }
        inventory.SetMissionItems();
        */
    }

    // Resets the items collected
    public void ClearAmount(){
        for(int i = 0; i < inventory.numberOfEachMissionItem.Count; i++){
            inventory.numberOfEachMissionItemCollected[i] = 0;
        }
    }

}

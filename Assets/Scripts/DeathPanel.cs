using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    private GameObject gm;
    private StateManager stateManager;

    public GameObject timePlayed;
    public GameObject EnemiesDefeated;
    public GameObject PlanetsDiscoverd;
    public GameObject ItemsCollected;
    // Options once dead
    private Button retryBtn;
    private Button backBtn;

    void Start(){
        gm = GameObject.Find("GameManager");
        stateManager = gm.GetComponent<StateManager>();
        // Death panel buttons
        retryBtn = gameObject.transform.GetChild(0).GetComponent<Button>();
        backBtn = gameObject.transform.GetChild(1).GetComponent<Button>();

        retryBtn.onClick.AddListener(RestartLevel);
        backBtn.onClick.AddListener(BackToMenu);
    }

    // Shows the statistics for the level
    public void Die(){
        // Rewrites for better format
        float i = GlobalManager.instance.timePlayed;
        int minute = Convert.ToInt32(Math.Floor(i/60f));
        int seconds = Convert.ToInt32(i - minute*60);
        // Adds 0 if needed
        if (seconds < 10){
            timePlayed.GetComponent<TMP_Text>().text = minute + ":0" + seconds;
        }
        else{
            timePlayed.GetComponent<TMP_Text>().text = minute + ":" + seconds;
        }
        
        EnemiesDefeated.GetComponent<TMP_Text>().text = GlobalManager.instance.enemiesDefeated.ToString();
        PlanetsDiscoverd.GetComponent<TMP_Text>().text = GlobalManager.instance.planetsDiscovered.ToString();
        ItemsCollected.GetComponent<TMP_Text>().text = GlobalManager.instance.itemsCollected.ToString();
    }

    // Returns to the menu screen
    public void BackToMenu(){
        stateManager.ChangeSceneByName("TitlePage");
    }

    // Resets current level
    public void RestartLevel(){
        StateManager.ReloadCurrentScene();
    }
}

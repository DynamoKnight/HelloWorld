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

    private GameObject statsPanel;
    // Options once dead
    private Button retryBtn;
    private Button backBtn;

    void OnEnable(){
        gm = GameObject.Find("GameManager");
        stateManager = gm.GetComponent<StateManager>();

        // Death panel buttons
        backBtn = transform.GetChild(0).GetComponent<Button>();
        retryBtn = transform.GetChild(1).GetComponent<Button>();
        // Make sure order is right
        statsPanel = transform.GetChild(4).gameObject;
        
        backBtn.onClick.AddListener(BackToMenu);
        retryBtn.onClick.AddListener(RestartLevel);
    }

    // Shows the statistics for the level
    public void Die(){
        // Writes the current stats into these game objects
        GlobalManager.instance.WriteStats(statsPanel);
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

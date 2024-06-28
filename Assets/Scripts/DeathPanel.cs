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

    private GameObject statsPanel;
    // Options once dead
    private Button retryBtn;
    private Button backBtn;

    void OnEnable(){
        gm = GameObject.Find("GameManager");

        // Death panel buttons
        backBtn = transform.GetChild(0).GetComponent<Button>();
        retryBtn = transform.GetChild(1).GetComponent<Button>();
        // Make sure order is right
        statsPanel = transform.GetChild(4).gameObject;
        
        backBtn.onClick.AddListener(StateManager.ReturnHome);
        retryBtn.onClick.AddListener(StateManager.RestartLevel);
    }

    // Shows the statistics for the level
    public void Die(){
        // Writes the current stats into these game objects
        GlobalManager.instance.WriteStats(statsPanel);
    }

}

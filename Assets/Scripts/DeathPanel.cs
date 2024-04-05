using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public GameObject gameManager;

    public GameObject timePlayed;
    public GameObject EnemiesDefeated;
    public GameObject PlanetsDiscoverd;
    public GameObject ItemsCollected;

    private Button retryBtn;
    private Button backBtn;

    void Start(){
        retryBtn = gameObject.transform.GetChild(1).GetComponent<Button>();
        backBtn = gameObject.transform.GetChild(2).GetComponent<Button>();

        retryBtn.onClick.AddListener(RetryLevel);
        backBtn.onClick.AddListener(BackToMenu);
    }

    public void Die(){
        float i = GlobalManager.instance.timePlayed;
        int minute = Convert.ToInt32(Math.Floor(i/60f));
        int seconds = Convert.ToInt32(i - minute*60);

        timePlayed.GetComponent<TextMeshProUGUI>().text = minute + ":" + seconds;
        EnemiesDefeated.GetComponent<TextMeshProUGUI>().text = GlobalManager.instance.enemiesDefeated.ToString();
        PlanetsDiscoverd.GetComponent<TextMeshProUGUI>().text = GlobalManager.instance.planetsDiscovered.ToString();
        ItemsCollected.GetComponent<TextMeshProUGUI>().text = GlobalManager.instance.itemsCollected.ToString();
    }

    void BackToMenu(){
        StateManager sm = gameManager.GetComponent<StateManager>();
        sm.ChangeSceneByName("TitlePage");
    }

    // Resets current level
    void RetryLevel(){
        StateManager.ReloadCurrentScene();
    }
}

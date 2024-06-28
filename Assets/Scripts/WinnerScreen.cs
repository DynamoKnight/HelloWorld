using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    private GameObject gm;
    private StateManager stateManager;

    private Image planetImage;
    private Button menuBtn;
    private Button nextBtn;
    // Stats panel texts
    private GameObject statsPanel;

    // The next level 
    [SerializeField] private string destination;

    public AudioSource winsound;

    // OnEnable is called right when it is set active
    void OnEnable(){
        gm = GameObject.Find("GameManager");
        stateManager = gm.GetComponent<StateManager>();

        planetImage = transform.GetChild(0).GetComponent<Image>();
        menuBtn = transform.GetChild(1).GetComponent<Button>();
        nextBtn = transform.GetChild(2).GetComponent<Button>();
        statsPanel = transform.GetChild(4).gameObject;

        // Sets the image to the planet that was completed
        planetImage.sprite = GlobalManager.instance.GetImages()[LevelManager.instance.GetIdxOfCurrentPlanet()]; 
        menuBtn.onClick.AddListener(LoadMenu);
        nextBtn.onClick.AddListener(NextLevel);

        winsound.volume = GameObject.Find("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;
    }

    // Sets itself active and records the stats
    public void ShowScreen(){
        // Pauses game so no spawning or anything happens
        LevelManager.instance.isFunctional = false;

        gameObject.SetActive(true);
        GlobalManager.instance.WriteStats(statsPanel);
        winsound.Play();
    }

    // Goes back home
    public void LoadMenu(){
        LevelManager.instance.UpdateStats();
        StateManager.ChangeSceneByName("LevelSelection");
        
    }

    // Goes to the desired planet
    public void NextLevel(){
        LevelManager.instance.UpdateStats();
        stateManager.LoadNextScene();
    }

}

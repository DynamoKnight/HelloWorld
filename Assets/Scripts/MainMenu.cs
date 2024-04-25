using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameObject gm;
    private StateManager stateManager;
    private Button playBtn;
    private Button mpBtn;
    private Button optionsBtn;
    private Button infoBtn;
    private Button quitBtn;

    void Start(){
        gm = GameObject.Find("GameManager");
        stateManager = gm.GetComponent<StateManager>();
        
        // Gets children buttons
        playBtn = gameObject.transform.GetChild(0).GetComponent<Button>();
        mpBtn = gameObject.transform.GetChild(1).GetComponent<Button>();
        optionsBtn = gameObject.transform.GetChild(2).GetComponent<Button>();
        infoBtn = gameObject.transform.GetChild(3).GetComponent<Button>();
        quitBtn = gameObject.transform.GetChild(4).GetComponent<Button>();

        // Adds event listeners
        playBtn.onClick.AddListener(PlayGame);
        mpBtn.onClick.AddListener(PlayMultiplayer);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public void PlayGame(){
        stateManager.ChangeSceneByName("LevelSelection");
    }

    public void PlayMultiplayer(){
        stateManager.ChangeSceneByName("Multiplayer");
    }

    void Update(){
        // Escape key
        if (Input.GetKeyDown(KeyCode.Escape)){
            QuitGame();
        }
    }

    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

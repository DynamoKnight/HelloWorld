using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;
    private StateManager stateManager;
    private Button playBtn;
    private Button optionsBtn;
    private Button quitBtn;

    void Start(){
        stateManager = gameManager.GetComponent<StateManager>();
        // Gets children buttons
        playBtn = gameObject.transform.GetChild(0).GetComponent<Button>();
        optionsBtn = gameObject.transform.GetChild(1).GetComponent<Button>();
        quitBtn = gameObject.transform.GetChild(2).GetComponent<Button>();

        // Adds event listeners
        playBtn.onClick.AddListener(PlayGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public void PlayGame(){
        SceneManager.LoadScene("Cutscene");
        //stateManager.ChangeSceneByName("Cutscene");
        //SceneManager.LoadScene("Game");
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

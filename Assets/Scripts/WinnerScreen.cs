using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{

    public Button MainMenuButton;

    public AudioSource winsound;

    private GameObject gm;
    private StateManager stateManager;

    // Start is called before the first frame update
    void Start(){
        gm = GameObject.Find("GameManager");
        stateManager = gm.GetComponent<StateManager>();
        
        MainMenuButton.onClick.AddListener(LoadMenu);
    }

    public void LoadMenu(){
        stateManager.ChangeSceneByName("TitlePage");
    }

}

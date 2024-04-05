using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{

    public Button MainMenuButton;

    public AudioSource winsound;

    [SerializeField] private GameObject gameManager;
    private StateManager stateManager;

    // Start is called before the first frame update
    void Start(){
        stateManager = gameManager.GetComponent<StateManager>();
        MainMenuButton.onClick.AddListener(LoadMenu);
    }

    public void LoadMenu(){
        stateManager.ChangeSceneByName("TitlePage");
    }

}

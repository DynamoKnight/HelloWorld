using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    GameObject gm;
    UIManager uiManager;

    private GameObject journalPanel;

    void Start(){
        gm = GameObject.Find("GameManager");
        uiManager = gm.GetComponent<UIManager>();
    }

    void Update(){
        if (LevelManager.instance.isFunctional){
            // J key is shortcut
            if (Input.GetKeyDown(KeyCode.J)){
                ClickJournalBtn();
            }
        }
    }

    // Pauses game and toggles Journal Panel
    public void ClickJournalBtn(){
        // Stops all time operations
        LevelManager.instance.TogglePause();
        uiManager.ToggleJournalPanel();
    }

}

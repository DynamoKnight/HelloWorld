using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    GameObject gm;
    private GameObject journalPanel;

    void Start(){
        gm = GameObject.Find("GameManager");
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
        UIManager _ui = gm.GetComponent<UIManager>();
        if (_ui){
            _ui.ToggleJournalPanel();
        }
    }

}

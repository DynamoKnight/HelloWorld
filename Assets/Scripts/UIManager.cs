using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages what UI is shown
public class UIManager : MonoBehaviour
{
    GameObject gm;
    private StateManager stateManager;
    
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject journalPanel;
    [SerializeField] GameObject avatar;
    [SerializeField] Sprite[] avatarImages;
    private float avatarAnimTime = 5f;
    private float timer = 0f;
    
    // Pause Menu
    [SerializeField] private GameObject pauseMenu;
    private Button quitBtn;
    private Button backBtn;

    void Start(){
        gm = GameObject.FindGameObjectWithTag("GameManager");
        stateManager = gm.GetComponent<StateManager>();
        
        // If the current scene is a planet, it will have escape buttons
        if(LevelManager.instance.levels.Contains(SceneManager.GetActiveScene().name)){
            // Pause menu buttons
            quitBtn = pauseMenu.transform.GetChild(0).GetComponent<Button>();
            backBtn = pauseMenu.transform.GetChild(1).GetComponent<Button>();
            // Adds functions to it
            backBtn.onClick.AddListener(TogglePauseMenu);
            quitBtn.onClick.AddListener(LevelManager.instance.QuitGame);
        }
        
    }

    void Update(){
        // Pauses game when escaped
        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePauseMenu();
        }

        // Changes avatar image breifly
        if(timer >= avatarAnimTime){
            timer = 0f;
        }
        else{
            timer += Time.deltaTime;
        }
    }
    
    // Shows/hides the pause menu
    public void TogglePauseMenu(){
        LevelManager.instance.TogglePause();
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }

    // Shows/hides the deathPanel
    public void ToggleDeathPanel(){
        // Sets to whatever it is not for toggling
        deathPanel.SetActive(!deathPanel.activeSelf);
        StartCoroutine(FadeDeath());
    }

    // Shows/hides the journal panel
    public void ToggleJournalPanel(){
        journalPanel.SetActive(!journalPanel.activeSelf);
    }

    // Makes the Death Screen more red over time
    public IEnumerator FadeDeath(){
        for (float red = 0f; red <= 0.75f; red += 0.015f){
            // The color values are from 0 to 1, not 0 to 255
            deathPanel.GetComponent<Image>().color = new Color(red, 0, 0, 200f/250f);
            yield return new WaitForSeconds(.1f);
        }
        
    }

    // Returns the back button
    public Button GetBackBtn(){
        return backBtn;
    }
}

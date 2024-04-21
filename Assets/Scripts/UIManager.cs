using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages what UI is shown
public class UIManager : MonoBehaviour
{
    GameObject gm;
    private StateManager stateManager;
    
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject journalPanel;
    [SerializeField] private GameObject startScreen;

    // Avatar
    private GameObject player;
    [SerializeField] GameObject avatar;
    [SerializeField] Sprite[] avatarImages;
    private float avatarAnimTime = 5f;
    private float timer = 0f;
    
    // Pause Menu
    [SerializeField] private GameObject pauseMenu;
    private Button quitBtn;
    private Button backBtn;

    // Timer
    [SerializeField] private GameObject gameTime; 

    void Start(){
        player = GameObject.Find("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager");
        stateManager = gm.GetComponent<StateManager>();

        player.SetActive(false);
        
        // If the current scene is a planet, it will have escape buttons
        if(LevelManager.instance.levels.Contains(SceneManager.GetActiveScene().name)){
            // Pause menu buttons
            quitBtn = pauseMenu.transform.GetChild(0).GetComponent<Button>();
            backBtn = pauseMenu.transform.GetChild(1).GetComponent<Button>();
            // Adds functions to it
            backBtn.onClick.AddListener(TogglePauseMenu);
            quitBtn.onClick.AddListener(LevelManager.instance.QuitGame);

            // Shows start screen
            StartScreen();

        }
        
    }

    void Update(){
        // Pauses game when escaped
        if (Input.GetKeyDown(KeyCode.Escape)){
            TogglePauseMenu();
        }
        // Game must be unpaused an functional
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Changes avatar image breifly
            if(timer >= avatarAnimTime){
                timer = 0f;
            }
            else{
                timer += Time.deltaTime;
            }

            // Updates the time
            float i = GlobalManager.instance.timePlayed;
            int minute = Convert.ToInt32(Math.Floor(i/60f));
            int seconds = Convert.ToInt32(i - minute*60);
            // Adds 0 if needed
            if (seconds < 10){
                gameTime.GetComponent<TMP_Text>().text = minute + ":0" + seconds;
            }
            else{
                gameTime.GetComponent<TMP_Text>().text = minute + ":" + seconds;
            }
        }
    }

    // Shows the start screen and initializes values
    public void StartScreen(){
        startScreen.SetActive(true);
        Dictionary<string, string[]> infos = GlobalManager.instance.GetPlanets();
        Sprite[] images = GlobalManager.instance.GetImages();
        // Make sure hierarchy is in order!
        Image image = startScreen.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        TMP_Text planet = startScreen.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        TMP_Text difficulty = startScreen.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        TMP_Text quote = startScreen.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>();
        TMP_Text info = startScreen.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>();
        TMP_Text enemy_info = startScreen.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>();

        // Gets the index of the current planet
        string currentPlanet = LevelManager.instance.currentPlanet;
        int currentPlanetIdx = infos.Keys.ToList().IndexOf(currentPlanet);
        // Sets values based on the planet
        image.sprite = images[currentPlanetIdx];
        planet.text = currentPlanet;
        difficulty.text = infos[currentPlanet][0];
        if (difficulty.text == "Easy"){
            difficulty.color = Color.green;
        }
        if (difficulty.text == "Medium"){
            difficulty.color = Color.yellow;
        }
        else if (difficulty.text == "Hard"){
            // Orange
            difficulty.color = new Color(1, 0.4f, 0);
        }
        else if (difficulty.text == "Extreme"){
            difficulty.color = Color.red;
        }
        else if (difficulty.text == "Boss"){
            // Purple
            difficulty.color = new Color(0.76f, 1, 0.4f);
        }
        quote.text = infos[currentPlanet][1];
        info.text = infos[currentPlanet][2];
        enemy_info.text = "Enemies: " + infos[currentPlanet][3];
        
        // The button to escape start screen
        Button startBtn = startScreen.transform.GetChild(1).GetComponent<Button>();
        startBtn.onClick.AddListener(StartLevel);
    }

    // Disables the startScreen and enables player
    public void StartLevel(){
        startScreen.SetActive(false);
        player.SetActive(true);
        LevelManager.instance.isFunctional = true;
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

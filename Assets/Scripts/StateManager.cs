using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the State of the Scene
public class StateManager : MonoBehaviour
{
    // The length of the cutscene
    public float loadingTime = 10f;
    public string nextScene;
    // Tutorial page
    public GameObject infoScreen;
    private Button startBtn;

    private CutsceneManager cm;
    private bool hasEntered = false;

    // Loads the Current Scene
    public static void ReloadCurrentScene(){
        // Uses index of scene, but can also use the name
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Loads the Scene by the name
    public static void ChangeSceneByName(string name){
        if (name != null){
            SceneManager.LoadScene(sceneName: name);
        }
    }

    public void ChangeToLoadingScreen(string planet){
        nextScene = planet;
        SceneManager.LoadScene("Loading");
    }

    public static void LoadInstructions(string planet){
        Debug.Log(planet + "Dialog");
        ChangeSceneByName(planet + "Dialog");
    }

    public void LoadNextScene(){
        ChangeSceneByName(nextScene);
    }

    public void LoadMap(){
        ChangeSceneByName("Loading");
    }

    // Goes back home without saving progress
    public static void ReturnHome(){
        PlayerStats.ResetGame();
        ChangeSceneByName("TitlePage");
    }

    // Resets current level
    public static void RestartLevel(){
        ReloadCurrentScene();
    }

    // Starts the planet level with cutscenes
    public static void GoToPlanet(string planet){
        if (planet == "Pluto"){
            ChangeSceneByName("Cutscene");
        }
        else if (planet == "NeptuneCutscene"){
            ChangeSceneByName("NeptuneCutscene");
        }
        else{
            LoadInstructions(planet);
        }
    }

    void Update(){
        if (!LevelManager.instance.isPaused){
            // Cutscenes
            GameObject cm_obj = GameObject.Find("Cutscene");
            if (cm_obj){
                cm = cm_obj.GetComponent<CutsceneManager>();
                // The first cutscene in the game
                if (SceneManager.GetActiveScene().name == "Cutscene"){
                        if (cm.IsCutsceneEnded() && !hasEntered){
                            // So the if statement wont return here
                            hasEntered = true;
                            // Once the cutscene ends, the tutorial screen is shown
                            infoScreen.SetActive(true);
                            startBtn = infoScreen.transform.GetChild(0).GetComponent<Button>();
                            startBtn.onClick.AddListener(() => ChangeSceneByName(nextScene));
                        }
                }
                if (SceneManager.GetActiveScene().name == "NeptuneCutscene"){
                    if (cm.IsCutsceneEnded()){
                        // So the if statement wont return here
                        hasEntered = true;
                        // Once the cutscene ends, the scene changes
                        ChangeSceneByName(nextScene);
                    }
                }
            }
        }
    }

}

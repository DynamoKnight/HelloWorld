using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the State of the Scene
public class StateManager : MonoBehaviour
{
    // The length of the cutscene
    public float cutsceneTime;
    private float loadingTime = 10f;
    public string nextScene;
    // Tutorial page
    public GameObject infoScreen;
    private Button startBtn;

    [SerializeField] private TimelineManager timelineManager;
    private bool cutsceneEnded = false;

    // Loads the Current Scene
    public static void ReloadCurrentScene(){
        // Uses index of scene, but can also use the name
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Loads the Scene by the name
    public void ChangeSceneByName(string name){
        if (name != null){
            SceneManager.LoadScene(sceneName: name);
        }
    }

    public void ChangeToLoadingScreen(string planet){
        nextScene = planet;
        SceneManager.LoadScene("Loading");
    }

    public void LoadInstructions(string planet){
        Debug.Log(planet + "Dialog");
        ChangeSceneByName(planet + "Dialog");
    }

    public void LoadNextScene(){
        ChangeSceneByName(nextScene);
    }

    void Update(){
        if (!LevelManager.instance.isPaused){
            // Time until cutscene ends
            if (SceneManager.GetActiveScene().name == "Cutscene"){
                if (timelineManager.GetTime() >= cutsceneTime && !cutsceneEnded){
                    cutsceneEnded = true;
                    // Once the cutscene ends, the scene changes
                    infoScreen.SetActive(true);
                    startBtn = infoScreen.transform.GetChild(0).GetComponent<Button>();
                    startBtn.onClick.AddListener(() => ChangeSceneByName(nextScene));
                }
            }
            if (SceneManager.GetActiveScene().name == "NeptuneCutscene"){
                if (timelineManager.GetTime() >= cutsceneTime && !cutsceneEnded){
                    cutsceneEnded = true;
                    // Once the cutscene ends, the scene changes
                    ChangeSceneByName(nextScene);
                }
            }
            // Time until cutscene ends
            if (SceneManager.GetActiveScene().name == "Loading"){
                loadingTime -= Time.deltaTime;
                // Once the cutscene ends, the scene changes
                if(loadingTime <= 0){
                    loadingTime = 10f;
                    ChangeSceneByName(nextScene);
                }
            }
        }
        
    }

}

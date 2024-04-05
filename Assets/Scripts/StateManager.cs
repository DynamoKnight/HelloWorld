using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the State of the Scene
public class StateManager : MonoBehaviour
{
    // The length of the cutscene
    public float cutsceneTime;
    private float loadingTime = 10f;
    public string nextScene;

    void Start(){
    }

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
        ChangeSceneByName(planet + "Dialog");
    }

    void Update(){
        if (!LevelManager.instance.isPaused){
            // Time until cutscene ends
            if(SceneManager.GetActiveScene().name == "Cutscene"){
                cutsceneTime -= Time.deltaTime;
                // Once the cutscene ends, the scene changes
                if(cutsceneTime <= 0){
                    ChangeSceneByName(nextScene);
                }
            }
            // Time until cutscene ends
            if(SceneManager.GetActiveScene().name == "Loading"){
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

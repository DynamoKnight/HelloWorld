using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
 
public class TimelineManager : MonoBehaviour
 {
    private GameObject gm;
    private UIManager uiManager;
    private StateManager stateManager;
    // A component of a timeline gameobject
    private PlayableDirector playableDirector;
    // The keyframe position to go to
    private int currentMarker = 0;
    // The time is in seconds
    public float[] markerTimes;
    
 
    void Start(){
        gm = GameObject.Find("GameManager");
        uiManager = gm.GetComponent<UIManager>();
        stateManager = gm.GetComponent<StateManager>();

        playableDirector = gameObject.GetComponent<PlayableDirector>();
    }
 
    void Update(){
        // ERROR MESSAGE, but still works:
        /*SerializedObjectNotCreatableException: Object at index 0 is null
        UnityEditor.Editor.CreateSerializedObject () (at <78fe3e0b66cf40fc8dbec96aa3584483>:0)
        UnityEditor.Editor.GetSerializedObjectInternal () (at <78fe3e0b66cf40fc8dbec96aa3584483>:0)
        UnityEditor.Editor.get_serializedObject () (at <78fe3e0b66cf40fc8dbec96aa3584483>:0)*/

        // When space is pressed, it will go to the desired keyframe position
        if (Input.GetKeyDown(KeyCode.Space)){
            // Starts the level immediately
            if (currentMarker == markerTimes.Length){
                uiManager.StartLevel();
            }
            // Sets the time
            else if (currentMarker < markerTimes.Length){
                float time = markerTimes[currentMarker];
                // Checks if it is in the future
                if (playableDirector.time < time){
                    playableDirector.time = time;
                }
                // The next time space is pressed, it will be ready to skip to the next keyframe
                currentMarker += 1;
            }
            
        } 
           
    }

    public float GetTime(){
        return (float)playableDirector.time;
    }

}
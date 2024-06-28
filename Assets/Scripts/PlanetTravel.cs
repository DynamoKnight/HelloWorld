using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetTravel : MonoBehaviour
{
    private GameObject gm;

    // Start is called before the first frame update
    void Start(){
        gm = GameObject.Find("GameManager");

        int cur = LevelManager.instance.GetIdxOfCurrentPlanet();
        // Loops through each planet button
        for (int i = 0; i < transform.childCount; i++){
            Button planetBtn = transform.GetChild(i).GetComponent<Button>();
            // If the level hasn't been reached yet, it can't be accessed
            if (i > LevelManager.instance.GetIdxOfBestPlanet()){
                // Disables button
                planetBtn.onClick.RemoveAllListeners();
                // Shows lock
                planetBtn.transform.GetChild(0).gameObject.SetActive(true);
            }
            // The level can be accessed
            else{
                // Lambda function allows parameter to be passed
                string planet = LevelManager.instance.levels[i];
                planetBtn.onClick.AddListener(() => GoPlanet(planet));
                // Hides lock
                planetBtn.transform.GetChild(0).gameObject.SetActive(false);
            }
            // Hides the spaceship if not on current planet
            if (i != cur){
                planetBtn.transform.GetChild(1).gameObject.SetActive(false);
            }
            // Shows a spaceship to indicate the current planet
            else if (i == cur){
                planetBtn.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    void GoPlanet(string planetName){
        StateManager.ChangeSceneByName(planetName);
    }
}

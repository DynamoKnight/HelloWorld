using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button statsBtn;
    [SerializeField] private GameObject leaderboard;

    private GameObject gm;
    private StateManager sm;

    // Start is called before the first frame update
    void Start(){
        gm = GameObject.Find("GameManager");
        sm = gm.GetComponent<StateManager>();

        backBtn.onClick.AddListener(ReturnHome);
        statsBtn.onClick.AddListener(ToggleLeaderboard);
    }

    // Update is called once per frame
    void LateUpdate(){
        for (int c = 0; c < transform.childCount; c++){
            Button planetBtn = transform.GetChild(c).GetComponent<Button>();
            if (c > GlobalManager.instance.GetIdxOfCurrentPlanet()){
                // Disables button
                planetBtn.onClick.RemoveAllListeners();
                // Shows lock
                planetBtn.transform.GetChild(2).gameObject.SetActive(true);
            }
            else{
                // Lambda function allows parameter to be passed
                planetBtn.onClick.AddListener(() => GoToPlanet(planetBtn.transform.GetChild(0).GetComponent<TMP_Text>().text));
                // Hides lock
                planetBtn.transform.GetChild(2).gameObject.SetActive(false);
                // Records the time
                float bestTime =  PlayerStats.BestTimes[c];
                planetBtn.transform.GetChild(1).GetComponent<TMP_Text>().text = "Best Score: " + GlobalManager.instance.FormatTime(bestTime);
            }
        }
        // Edits leaderboard values
        GameObject enemiesDefeated = leaderboard.transform.GetChild(1).GetChild(0).gameObject;
        GameObject totalViolations = leaderboard.transform.GetChild(2).GetChild(0).gameObject;

        enemiesDefeated.GetComponent<TMP_Text>().text = PlayerStats.EnemiesDefeated.ToString();
        totalViolations.GetComponent<TMP_Text>().text = PlayerStats.TotalViolations.ToString();


    }

    // Starts the planet level
    private void GoToPlanet(string planet){
        if (planet == "Pluto"){
            sm.ChangeSceneByName("Cutscene");
        }
        else if (planet == "NeptuneCutscene"){
            sm.ChangeSceneByName("NeptuneCutscene");
        }
        else{
            sm.LoadInstructions(planet);
        }
    }

    // Goes to title page
    private void ReturnHome(){
        sm.ChangeSceneByName("TitlePage");
    }

    // Turns on/off leaderboard
    private void ToggleLeaderboard(){
        leaderboard.SetActive(!leaderboard.activeSelf);
    }

}

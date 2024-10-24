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

    // Start is called before the first frame update
    void Start(){
        gm = GameObject.Find("GameManager");

        backBtn.onClick.AddListener(StateManager.ReturnHome);
        statsBtn.onClick.AddListener(ToggleLeaderboard);

        for (int i = 0; i < transform.childCount; i++){
            Button planetBtn = transform.GetChild(i).GetComponent<Button>();
            // Create a local copy of i
            int idx = i;
            // Lambda function allows parameter to be passed
            // a local copy has to be passed because when the lambda executes,
            // the original variable points to it's final value.
            planetBtn.onClick.AddListener(() => AttemptGoToPlanet(idx));
        }
    }


    void LateUpdate(){
        for (int i = 0; i < transform.childCount; i++){
            Button planetBtn = transform.GetChild(i).GetComponent<Button>();
            if (i <= LevelManager.instance.GetIdxOfBestPlanet()){
                // Hides lock
                planetBtn.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        // Edits leaderboard values
        GameObject enemiesDefeated = leaderboard.transform.GetChild(1).GetChild(0).gameObject;
        GameObject totalViolations = leaderboard.transform.GetChild(2).GetChild(0).gameObject;

        enemiesDefeated.GetComponent<TMP_Text>().text = PlayerStats.EnemiesDefeated.ToString();
        totalViolations.GetComponent<TMP_Text>().text = PlayerStats.TotalViolations.ToString();

    }

    // Checks if the planet has been reached before sending player to planet
    void AttemptGoToPlanet(int idx){
        Debug.Log(idx + " | " + LevelManager.instance.GetIdxOfBestPlanet());
        // The planet is a reached level
        if (idx <= LevelManager.instance.GetIdxOfBestPlanet()){
            Debug.Log("We outta here");
            // Goes to planet
            StateManager.GoToPlanet(LevelManager.instance.levels[idx]);
        }
    }

    // Turns on/off leaderboard
    private void ToggleLeaderboard(){
        StateManager.ChangeSceneByName("Statistics");
        //leaderboard.SetActive(!leaderboard.activeSelf);
    }

}

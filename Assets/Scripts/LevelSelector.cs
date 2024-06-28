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
    }


    void LateUpdate(){
        for (int i = 0; i < transform.childCount; i++){
            Button planetBtn = transform.GetChild(i).GetComponent<Button>();
            if (i > LevelManager.instance.GetIdxOfBestPlanet()){
                // Disables button
                planetBtn.onClick.RemoveAllListeners();
                // Shows lock
                planetBtn.transform.GetChild(2).gameObject.SetActive(true);
            }
            else{
                // Lambda function allows parameter to be passed
                planetBtn.onClick.AddListener(() => StateManager.GoToPlanet(LevelManager.instance.levels[i]));
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

    

    // Turns on/off leaderboard
    private void ToggleLeaderboard(){
        leaderboard.SetActive(!leaderboard.activeSelf);
    }

}

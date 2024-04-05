using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spaceship : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;

    [SerializeField]
    private Player player;
    [SerializeField]
    private float activationDistance = 5f;
    [SerializeField]
    private GameObject leaveMenu;
    private Button leaveBtn;
    private Button backBtn;

    [SerializeField]
    private GameObject winnerScreen;

    [SerializeField]
    private string destination;

    private bool exited = false;

    public Inventory inventory;

    void Start(){
        leaveBtn = leaveMenu.transform.GetChild(1).GetComponent<Button>();
        backBtn = leaveMenu.transform.GetChild(2).GetComponent<Button>();
        // Goes to new level
        leaveBtn.onClick.AddListener(LeaveGame);
        // Exits screen
        backBtn.onClick.AddListener(LeaveScreen);

    }

    // Goes to the next level if all mission tasks are fulfilled
    public void LeaveGame(){
        if (LevelManager.instance.levelComplete == true){
            // Pauses game so no spawning or anything happens
            StateManager sm = gameManager.GetComponent<StateManager>();
            if (destination == "WinnerScreen"){
                // Shows the winner screen
                winnerScreen.SetActive(true);
            }
            else{
                // Loads the instructions scene
                sm.LoadInstructions(destination);
            }
            
        }
    }

    void Update(){        
        // Player should be alive first
        if (player){
            // Calculate the distance between player and target object
            float distance = Vector2.Distance(player.GetComponent<Rigidbody2D>().position, gameObject.GetComponent<Rigidbody2D>().position);

            // Check if the player is within the activation distance
            if (distance <= activationDistance && inventory.missionItemsCollected){
                if(!exited){
                    // Enable the button
                    leaveMenu.SetActive(true);
                }
                
            }
            else{
                // Disable the button
                leaveMenu.SetActive(false);
                exited = false;
            }
        }
    }

    // Sets a flag that the user clicked the backBtn
    public void LeaveScreen(){
        leaveMenu.SetActive(false);
        exited = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spaceship : MonoBehaviour
{
    private GameObject gm;
    private InventoryManager inventoryManager;
    [SerializeField] private Player player;

    // Distance until the leave text appears
    [SerializeField] private float activationDistance = 5f;
    // Once aall resources are collected, this will appear
    [SerializeField] private GameObject leaveMenu;
    private Button leaveBtn;
    private Button backBtn;
    // Indicates whether the leave menu is canceled
    private bool exited = false;

    // Level complete menu
    [SerializeField] private GameObject winnerScreen;

    void Start(){
        gm = GameObject.Find("GameManager");
        inventoryManager = gm.GetComponent<InventoryManager>();

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
            LeaveScreen();
            winnerScreen.GetComponent<WinnerScreen>().ShowScreen();
        }
    }

    void Update(){        
        // Player should be alive first
        if (player){
            // Calculate the distance between player and target object
            float distance = Vector2.Distance(player.GetComponent<Rigidbody2D>().position, gameObject.GetComponent<Rigidbody2D>().position);

            // Check if the player is within the activation distance
            if (distance <= activationDistance && inventoryManager.missionItemsCollected){
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

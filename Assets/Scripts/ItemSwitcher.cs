using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    private GameObject gm;
    private InventoryManager inventoryManager;
    public GameObject player;
    // List of all the items of the player (different from Inventory)
    private Transform playerInventory;

    // Keeps track of the item in use
    private GameObject currentItem;
    private GameObject previousItem;
    private int currentIdx;
    
    void Start(){
        gm = gameObject;
        inventoryManager = gm.GetComponent<InventoryManager>();
        playerInventory = player.transform.GetChild(0);

        currentIdx = 0;
    }

    void Update(){
        // Keeps trying to find an item in the inventory, and player must still be alive
        if (currentItem == null && player != null){
            try{
                // Sets the item in use based on the 0th item in the player's inventory
                currentItem = playerInventory.GetChild(0).gameObject;
                currentItem.GetComponent<SpriteRenderer>().enabled = true;
                if (currentItem.GetComponent<Weapon>() != null){
                    currentItem.GetComponent<Weapon>().beingUsed = true;
                }

                //Debug.Log($"Current Item: {currentItem}");
            }
            catch (ArgumentOutOfRangeException){
                Debug.Log("The dictionary is empty or does not have an element at the specified index.");
            }
        }
        // Only collects input if game is unpaused and functional
        // Won't work until there is an item in the Inventory
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional && currentItem != null){
            // Scroll wheel goes up to change item
            if(Input.mouseScrollDelta.y > 0){
                // If trying to go up the list but on last index, resets back to 0
                if(currentIdx >= playerInventory.childCount- 1){
                    previousItem = currentItem;
                    currentItem = playerInventory.GetChild(0).gameObject;
                    currentIdx = 0;
                    SwitchItem();
                }
                // Goes to the next item on the list
                else {
                    previousItem = currentItem;
                    currentItem = playerInventory.GetChild(currentIdx + 1).gameObject;
                    currentIdx++;
                    SwitchItem();
                }
                // Shows the border around the current item
                inventoryManager.HighlightSlot(currentIdx);
            }
            // Scroll wheel goes down to change item
            else if(Input.mouseScrollDelta.y < 0){
                // If trying to go down list but it is at 0, goes to the top of the list
                if(currentIdx <= 0){
                    previousItem = currentItem;
                    currentItem =  playerInventory.GetChild(playerInventory.childCount- 1).gameObject;
                    currentIdx = playerInventory.childCount- 1;
                    SwitchItem();
                }
                // Goes to the previous item on the list
                else {
                    previousItem = currentItem;
                    currentItem = playerInventory.GetChild(currentIdx - 1).gameObject;
                    currentIdx--;
                    SwitchItem();
                }
                // Shows the border around the current item
                inventoryManager.HighlightSlot(currentIdx);
            }
        }

    }

    // Enables/Disables the items in use
    private void SwitchItem(){
        //Debug.Log(previousItem.name + " | " + currentItem.name);
        previousItem.GetComponent<SpriteRenderer>().enabled = false;
        currentItem.GetComponent<SpriteRenderer>().enabled = true;
        // Enables/Disables Weapon script
        if (previousItem.GetComponent<Weapon>() != null){
            // Every weapon class is a child of the Weapon script
            previousItem.GetComponent<Weapon>().beingUsed = false;
        }
        if (currentItem.GetComponent<Weapon>() != null){
            currentItem.GetComponent<Weapon>().beingUsed = true;
        }

    }

    // Returns the current index of the item list
    public int GetCurrentIndex(){
        return currentIdx;
    }



}

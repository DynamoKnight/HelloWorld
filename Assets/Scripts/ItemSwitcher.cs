using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    private GameObject gm;
    private InventoryManager inventoryManager;
    public GameObject player;

    // The current items from the inventory
    private List<GameObject> itemList = new();
    // Keeps track of the item in use
    private GameObject currentItem;
    private GameObject previousItem;
    private int currentIdx;
    
    void Start(){
        gm = gameObject;
        inventoryManager = gm.GetComponent<InventoryManager>();

        // Sets the items in use based on the inventory
        itemList = inventoryManager.GetItemList();
        currentItem = itemList[0];
        currentIdx = 0;
    }

    void Update(){
        // Only collects input if game is unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Scroll wheel goes up to change item
            if(Input.mouseScrollDelta.y > 0){
                // If trying to go up the list but on last index, resets back to 0
                if(currentIdx == itemList.Count - 1){
                    previousItem = currentItem;
                    currentItem = itemList[0];
                    currentIdx = 0;
                    SwitchItem();
                }
                // Goes to the next item on the list
                else {
                    previousItem = currentItem;
                    currentItem = itemList[currentIdx + 1];
                    currentIdx++;
                    SwitchItem();
                }
                // Shows the border around the current item
                inventoryManager.HighlightSlot(currentIdx);
            }
            // Scroll wheel goes down to change item
            else if(Input.mouseScrollDelta.y < 0){
                // If trying to go down list but it is at 0, goes to the top of the list
                if(currentIdx == 0){
                    previousItem = currentItem;
                    currentItem = itemList[itemList.Count - 1];
                    currentIdx = itemList.Count - 1;
                    SwitchItem();
                }
                // Goes to the previous item on the list
                else {
                    previousItem = currentItem;
                    currentItem = itemList[currentIdx - 1];
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

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private GameObject gm;
    private Inventory inventory;
    public GameObject player;

    // The current weapons from the inventory
    private List<GameObject> weaponList = new();
    // Keeps track of the weapon in use
    private GameObject currentWeapon;
    private GameObject previousWeapon;
    private int currentIdx;
    
    void Start(){
        gm = GameObject.Find("GameManager");
        inventory = gm.GetComponent<Inventory>();

        // Sets the weapons in use based on the inventory
        weaponList = inventory.GetWeaponList();
        currentWeapon = weaponList[0];
        currentIdx = 0;
    }

    void Update(){
        // Only collects input if game is unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Scroll wheel goes up to change weapon
            if(Input.mouseScrollDelta.y > 0){
                // If trying to go up the list but on last index, resets back to 0
                if(currentIdx == weaponList.Count-1){
                    previousWeapon = currentWeapon;
                    currentWeapon = weaponList[0];
                    currentIdx = 0;
                    SwitchWeapon();
                }
                // Goes to the next weapon on the list
                else {
                    previousWeapon = currentWeapon;
                    currentWeapon = weaponList[currentIdx + 1];
                    currentIdx++;
                    SwitchWeapon();
                }
                // Shows the border around the current weapon
                inventory.HighlightSlot(currentIdx);
            }
            // Scroll wheel goes down to change weapon
            else if(Input.mouseScrollDelta.y < 0){
                // If trying to go down list but it is at 0, goes to the top of the list
                if(currentIdx == 0){
                    previousWeapon = currentWeapon;
                    currentWeapon = weaponList[weaponList.Count - 1];
                    currentIdx = weaponList.Count - 1;
                    SwitchWeapon();
                }
                // Goes to the previous weapon on the list
                else {
                    previousWeapon = currentWeapon;
                    currentWeapon = weaponList[currentIdx - 1];
                    currentIdx--;
                    SwitchWeapon();
                }
                // Shows the border around the current weapon
                inventory.HighlightSlot(currentIdx);
            }
        }

    }

    // Enables/Disables the weapons in use
    private void SwitchWeapon(){
        previousWeapon.GetComponent<SpriteRenderer>().enabled = false;
        // Every weapon class is a child of the Gun script
        previousWeapon.GetComponent<Gun>().beingUsed = false;

        currentWeapon.GetComponent<SpriteRenderer>().enabled = true;       
        currentWeapon.GetComponent<Gun>().beingUsed = true;

    }

    // Returns the current index of the weapon list
    public int GetCurrentIndex(){
        return currentIdx;
    }



}

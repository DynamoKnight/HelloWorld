using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Inventory inventory;

    public GameObject player;

    public GameObject LaserGun;
    private Gun laserGunScript;

    [SerializeField]
    public GameObject Scythe;
    private Scythe scytheScript;

    public GameObject FrostGun;

    List<string> weaponList = new();

    private string currentWeapon;
    private string pastWeapon;
    private int currentInd;
    
    void Start(){
        // Better way to get the script
        laserGunScript = LaserGun.GetComponent<Gun>();
        scytheScript = Scythe.GetComponent<Scythe>();

        weaponList = inventory.GetWeaponList();
        currentWeapon = weaponList[0];
        currentInd = 0;
    }

    void Update(){
        // Only collects input if game is unpaused
        if(!LevelManager.instance.isPaused){
            //if trying to go up the list but on last index, resets back to 0
            if(Input.mouseScrollDelta.y > 0){
                if(currentInd == weaponList.Count-1){
                    pastWeapon = currentWeapon;
                    currentWeapon = weaponList[0];
                    currentInd = 0;
                    SwitchWeapon();
                }
                else {
                    //goes up one on the list
                    pastWeapon = currentWeapon;
                    currentWeapon = weaponList[currentInd + 1];
                    currentInd++;
                    SwitchWeapon();
                }
            }

            else if(Input.mouseScrollDelta.y < 0){
                // If trying to go down list but it is at 0, goes to the top of the list
                if(currentInd == 0){
                    pastWeapon = currentWeapon;
                    currentWeapon = weaponList[weaponList.Count - 1];
                    currentInd = weaponList.Count - 1;
                    SwitchWeapon();
                }
                else {
                    // Goes one down on list
                    pastWeapon = currentWeapon;
                    currentWeapon = weaponList[currentInd - 1];
                    currentInd--;
                    SwitchWeapon();
                }
            }
        }
    }
    //switches the weapon on the screen
    private void SwitchWeapon()
    {
        nameToObject(pastWeapon).GetComponent<SpriteRenderer>().enabled = false;
        nameToObject(currentWeapon).GetComponent<SpriteRenderer>().enabled = true;
        EnableAndDisable(pastWeapon, currentWeapon);
    }
    //converts the name of the object to the game object
    private GameObject nameToObject(string name){
        if(name == "LaserGun"){
            return LaserGun;
        }

        else if(name == "Scythe"){
            return Scythe;
        }

        else if(name == "FrostGun"){
            return FrostGun;
        }
        else{
            return null;
        }
    }
    // Disables previous weapon and enables current weapon 
    private void EnableAndDisable(string pastName, string currentName){

        // Disables previous weapon (implement other weapon scripts)
        if(pastName == "LaserGun")
        {
            laserGunScript.beingUsed = false;
        }

        else if(pastName == "Scythe")
        {
            scytheScript.beingUsed = false;
        }

        else if(pastName == "FrostGun")
        {
            //laserGunScript.beingUsed = false;
        }

        // Enables current weapon (implement other weapon scripts)
        if(currentName == "LaserGun")
        {
            laserGunScript.beingUsed = true;
        }

        else if(currentName == "Scythe")
        {
            scytheScript.beingUsed = true;
        }

        else if(currentName == "FrostGun")
        {
            //laserGunScript.beingUsed = false;
        }
    }

}

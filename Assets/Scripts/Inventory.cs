using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public static class Inventory
{
    // A list of every prefab game object
    public static List<GameObject> Prefabs {get; set;}
    // Currency that saves
    public static int Credits {get; set;} = 0;
    // List of items(resources/weapons) and the number of them collected
    public static Dictionary<GameObject, int> Items {get; set;} = new();
    // Limits the size of the number of resources that can be collected
    public static int CarryingCapacity {get; set;} = 8;
    // The mission items across the entire game
    public static List<MissionItem> MissionItems {get; set;} = new();
    
    // All resources are removed
    public static void ClearResources(){
        Items = new Dictionary<GameObject, int>();
    }

    // Resets back to start
    public static void ResetInventory(){
        ClearResources();
        Credits = 0;
        CarryingCapacity = 0;
    }

    // Increments the count of the object collected
    // Returns if the item was added or not
    public static bool CollectItem(GameObject gameObject, int count){
        // Removes the "(Clone)" from the name
        string drop = gameObject.name.Replace("(Clone)", "");
        // Checks if already collected
        // Since every game object is different, is has to check by name
        foreach (GameObject resource in Items.Keys){
            if (drop == resource.name){
                // Weapons can only be added once
                if(!gameObject.CompareTag("Weapon")){
                    Items[resource] += count;
                    // Indicates item was added
                    GlobalManager.instance.itemsCollected++;
                }
                // Object was found in Inventory
                return true;
            }
        }
        // Reached here means the item was not found
        // Only creates if there is space
        if (Items.Keys.Count() < CarryingCapacity){
            // Initializes space
            GameObject resource = GetPrefabFromObject(gameObject);
            Items.Add(resource, 1);
            GlobalManager.instance.itemsCollected++;
            return true;
        }
        // No space left
        else{
            Debug.Log("Inventory Full!");
            return false;
        }
    }
    // Increments the count of the object collected by 1
    public static bool CollectItem(GameObject gameObject){
        return CollectItem(gameObject, 1);
    }

    // Decrements the count of the object collected
    // Returns if the item was lost or not
    public static bool LoseItem(GameObject gameObject, int count){
        // Checks if item is in inventory
        // Since every game object is different, is has to check by name
        foreach (GameObject resource in Items.Keys){
            if (gameObject.name == resource.name){
                // Checks if there is enough quantity
                if (Items[resource] >= count){
                    Items[resource] -= count;
                    return true;
                }
                else{
                    Debug.Log("Not Enough Items in Inventory!");
                    return false;
                }
            }
        }
        // Reaching here means item not found
        Debug.Log("Item not in Inventory!");
        return false;
    }

    // Adds Credits
    // Returns if the credits was gained or not
    public static bool CollectCredits(int number){
        Credits += number;
        return true;
    }

    // Removes Credits
    // Returns if the credits was removed or not
    public static bool LoseCredits(int number){
        if (Credits >= number){
            Credits -= number;
            return true;
        }
        else{
            Debug.Log("Not Enough Credits!");
            return false;
        }
    }

    // Returns whether an object is in the inventory
    public static bool Contains(GameObject gameObject){
        GameObject prefab = GetPrefabFromObject(gameObject);
        if (prefab != null){
            foreach (GameObject item in Items.Keys){
                if (prefab.name == item.name){
                    // Matches by name
                    return true;
                }
            }
        }
        // Object could not be found
        return false;
    }

    // Returns the original game object from a name 
    public static GameObject GetPrefabFromObject(GameObject gameObject){
        foreach (GameObject prefab in Prefabs){
            // Must be exactly equal, "(Clone)" is removed
            string objectName = gameObject.name.Replace("(Clone)", "");
            if (prefab.name == objectName){
                return prefab;
            }
        }
        // No object found
        return null;
    }

    // Gets weapons from Item list
    public static List<GameObject> GetWeaponList(){
        List<GameObject> weapons = new();
        // Looks only for weapons
        foreach (GameObject item in Items.Keys){
            if (item.CompareTag("Weapon")){
                weapons.Add(item);
            }
        }
        return weapons;
    }
    
}

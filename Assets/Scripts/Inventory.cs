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
    // List of weapons for the current inventory
    public static List<GameObject> Weapons {get; set;} = new();
    // List of resources and the number of them collected
    public static Dictionary<GameObject, int> Resources {get; set;} = new();
    // Limits the size of the number of resources that can be collected
    public static int CarryingCapacity {get; set;} = 10;
    
    // All resources are removed
    public static void ClearResources(){
        Resources = new Dictionary<GameObject, int>();
    }

    // Increments the count of the object collected
    // Returns if the item was added or not
    public static bool CollectItem(GameObject gameObject, int count){
        // Removes the "(Clone)" from the name
        string drop = gameObject.name.Replace("(Clone)", "");
        // Checks if already collected
        // Since every game object is different, is has to check by name
        foreach (GameObject resource in Resources.Keys){
            if (drop == resource.name){
                Resources[resource] += count;
                // Indicates item was added
                GlobalManager.instance.itemsCollected++;
                return true;
            }
        }
        // Reached here means the item was not found
        // Only creates if there is space
        if (Resources.Keys.Count() < CarryingCapacity){
            // Initializes space
            GameObject resource = GetPrefabFromObject(gameObject);
            Resources.Add(resource, 1);
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
        foreach (GameObject resource in Resources.Keys){
            if (gameObject.name == resource.name){
                // Checks if there is enough quantity
                if (Resources[resource] >= count){
                    Resources[resource] -= count;
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

    // Returns the original game object from a name 
    public static GameObject GetPrefabFromObject(GameObject gameObject){
        foreach (GameObject prefab in Prefabs){
            // Must be exactly equal, "(Clone)" is removed
            string drop = gameObject.name.Replace("(Clone)", "");
            if (prefab.name == drop){
                return prefab;
            }
        }
        // No object found
        return null;
    }
    
}

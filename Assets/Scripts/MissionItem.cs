using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows class to be manipulated in the Inspector
[System.Serializable]
public class MissionItem
{
    // The game object contains the name
    public GameObject gameObject;
    // The number of mission items required
    public int toCollect;
    // The position of the mission item in the resources list (side inventory panel)
    [HideInInspector]
    public int index;
    // Indicates whether all the required quantities have been collected
    public bool isCollected = false;

    public MissionItem(GameObject gameObject, int toCollect){
        this.gameObject = gameObject;
        this.toCollect = toCollect;
    }

    // Sets the position of the item
    public int SetIndex(int index){
        this.index = index;
        return this.index;
    }
    

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows class to be manipulated in the Inspector
[System.Serializable]
public class Dropper
{
    // Represents the object that is dropping the drops
    public GameObject alien;
    // The list of drops
    public GameObject[] drops;

    public Dropper(GameObject gameObject, GameObject[] drops){
        this.alien = gameObject;
        this.drops = drops;
    }
}
